import React, { useState, useEffect, useCallback } from 'react';
import BanksView from './View';
import { getCatalogBanks, getUserBanks, createUserBank } from './api';

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
 * Banks Page Controller
 * Manages the user's linked bank accounts and the public institution catalog.
 *
 * @returns {JSX.Element} The rendered Banks page controller.
 */
const Banks = () => {
  const [userBanks, setUserBanks] = useState([]);
  const [catalogBanks, setCatalogBanks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [modalOpen, setModalOpen] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [feedback, setFeedback] = useState(null);
  const [formData, setFormData] = useState({
    selectedBankId: '',
    nickname: '',
  });

  const fetchAll = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const [userRes, catalogRes] = await Promise.all([
        getUserBanks(),
        getCatalogBanks({ page: 1, page_size: 30 }),
      ]);
      const linked = extractList(userRes);
      const catalog = extractList(catalogRes);
      setUserBanks(linked);
      setCatalogBanks(catalog);
      if (catalog.length > 0) {
        setFormData((prev) => ({
          ...prev,
          selectedBankId: prev.selectedBankId || catalog[0].bank_id,
        }));
      }
    } catch (err) {
      console.error(err);
      setError('Unable to load bank accounts and institution catalog.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchAll();
  }, [fetchAll]);

  const handleOpenModal = (preselectedBankId = null) => {
    setFeedback(null);
    setFormData({
      selectedBankId: preselectedBankId || catalogBanks[0]?.bank_id || '',
      nickname: '',
    });
    setModalOpen(true);
  };

  const handleCloseModal = () => {
    setModalOpen(false);
  };

  const handleFormChange = (field, value) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const chosenBank = catalogBanks.find(
      (b) => Number(b.bank_id) === Number(formData.selectedBankId)
    );
    if (!chosenBank) {
      setFeedback({ type: 'error', message: 'Please select a valid banking institution.' });
      return;
    }

    try {
      setSubmitting(true);
      setFeedback(null);
      await createUserBank({
        bank_name: chosenBank.name,
        bank_type: Number(chosenBank.bank_type),
        nickname: formData.nickname.trim() || `${chosenBank.name} Account`,
      });
      setModalOpen(false);
      await fetchAll();
    } catch (err) {
      const apiMsg = err.response?.data?.messages?.[0] || 'Failed to link bank account.';
      setFeedback({ type: 'error', message: apiMsg });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <BanksView
      userBanks={userBanks}
      catalogBanks={catalogBanks}
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

export default Banks;
