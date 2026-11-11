import React, { useState, useEffect, useCallback } from 'react';
import BudgetView from './View';
import { getUserGoals, createUserGoal } from './api';

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
 * Budget & Goals Page Controller
 * Fetches and manages the state for user financial goals via /api/public/v1/User/goal.
 *
 * @returns {JSX.Element} The rendered Budget page controller.
 */
const Budget = () => {
  const [goals, setGoals] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [modalOpen, setModalOpen] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [feedback, setFeedback] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    amount: '',
    is_percent: false,
    expire_at: '2027-12-31',
  });

  const fetchGoals = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await getUserGoals({ page: 1, page_size: 20, active: true });
      setGoals(extractList(response));
    } catch (err) {
      console.error(err);
      setError('Failed to load financial goals and budget targets.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchGoals();
  }, [fetchGoals]);

  const handleOpenModal = () => {
    setFeedback(null);
    setFormData({
      name: '',
      description: '',
      amount: '',
      is_percent: false,
      expire_at: '2027-12-31',
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
      const isoExpire = new Date(`${formData.expire_at}T23:59:59Z`).toISOString();
      await createUserGoal({
        name: formData.name.trim(),
        description: formData.description.trim(),
        amount: Number(formData.amount),
        is_percent: Boolean(formData.is_percent),
        expire_at: isoExpire,
      });
      setModalOpen(false);
      await fetchGoals();
    } catch (err) {
      const msg = err.response?.data?.messages?.[0] || 'Failed to create financial goal.';
      setFeedback({ type: 'error', message: msg });
    } finally {
      setSubmitting(false);
    }
  };

  const totalMonetaryTarget = goals
    .filter((g) => !g.is_percent)
    .reduce((acc, g) => acc + (Number(g.amount) || 0), 0);

  return (
    <BudgetView
      goals={goals}
      totalMonetaryTarget={totalMonetaryTarget}
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

export default Budget;
