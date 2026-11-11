import React, { useState, useEffect, useCallback } from 'react';
import CategoriesView from './View';
import { getUserCategories, createUserCategory, getUserGoals } from './api';

/**
 * Extract array items safely from standard or paginated Core API responses.
 *
 * @param {Object} response - Raw Axios response object.
 * @returns {Array} Extracted list of items.
 */
const extractList = (response) => {
  const payload = response?.data?.data ?? response?.data ?? [];
  if (Array.isArray(payload)) return payload;
  if (Array.isArray(payload.items)) return payload.items;
  return [];
};

/**
 * Categories Page Controller
 * Manages user spending categories and enforces the exclusive Goal vs. Parent Category linkage rule.
 *
 * @returns {JSX.Element} The rendered Categories page controller.
 */
const Categories = () => {
  const [categories, setCategories] = useState([]);
  const [goals, setGoals] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [modalOpen, setModalOpen] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [feedback, setFeedback] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    linkMode: 'goal', // 'goal' or 'parent'
    goalId: '',
    parentCategoryId: '',
  });

  const fetchAll = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const [catRes, goalRes] = await Promise.all([
        getUserCategories({ page: 1, page_size: 30, active: true }),
        getUserGoals({ page: 1, page_size: 30, active: true }),
      ]);
      const catList = extractList(catRes);
      const goalList = extractList(goalRes);
      setCategories(catList);
      setGoals(goalList);
    } catch (err) {
      console.error(err);
      setError('Failed to load categories and goals.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchAll();
  }, [fetchAll]);

  const handleOpenModal = () => {
    setFeedback(null);
    setFormData({
      name: '',
      description: '',
      linkMode: goals.length > 0 ? 'goal' : 'parent',
      goalId: goals[0]?.id || '',
      parentCategoryId: categories[0]?.id || '',
    });
    setModalOpen(true);
  };

  const handleCloseModal = () => setModalOpen(false);

  const handleFormChange = (field, value) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      setSubmitting(true);
      setFeedback(null);
      const payload = {
        name: formData.name.trim(),
        description: formData.description.trim(),
        goal: formData.linkMode === 'goal' ? Number(formData.goalId) : null,
        parent_category: formData.linkMode === 'parent' ? Number(formData.parentCategoryId) : null,
      };
      await createUserCategory(payload);
      setModalOpen(false);
      await fetchAll();
    } catch (err) {
      const msg = err.response?.data?.messages?.[0] || 'Failed to create category.';
      setFeedback({ type: 'error', message: msg });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <CategoriesView
      categories={categories}
      goals={goals}
      loading={loading}
      error={error}
      modalOpen={modalOpen}
      submitting={submitting}
      feedback={feedback}
      formData={formData}
      onOpenModal={handleOpenModal}
      onCloseModal={handleCloseModal}
      onFormChange={handleFormChange}
      onSubmit={handleSubmit}
    />
  );
};

export default Categories;
