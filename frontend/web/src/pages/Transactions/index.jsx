import React, { useState, useEffect, useCallback } from 'react';
import TransactionsView from './View';
import {
  getPaymentTransactions,
  createPaymentTransaction,
  getTransactionStatuses,
  getTransactionSituations,
  getTransactionTypes,
  getPaymentSchedulers,
  getUserCategories,
  getUserBanks,
  getCatalogBanks,
  getUserCards,
} from './api';

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
 * Transactions Page Controller
 * Fetches and manages the state for the user's payment transaction history and creation modal.
 *
 * @returns {JSX.Element} The rendered Transactions page controller.
 */
const Transactions = () => {
  const [transactions, setTransactions] = useState([]);
  const [statuses, setStatuses] = useState([]);
  const [situations, setSituations] = useState([]);
  const [types, setTypes] = useState([]);
  const [schedulers, setSchedulers] = useState([]);
  const [categories, setCategories] = useState([]);
  const [banks, setBanks] = useState([]);
  const [cards, setCards] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [filters, setFilters] = useState({
    transaction_status_id: '',
    transaction_situation_id: '',
    transaction_type_id: '',
    category: '',
    user_bank_card: '',
  });

  const [modalOpen, setModalOpen] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [feedback, setFeedback] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    amount: '',
    pay_date: new Date().toISOString().slice(0, 10),
    category_name: '',
    transaction_status: '',
    transaction_situation: '',
    transaction_type: '',
    bank: '',
    card: 0,
    scheduler: 0,
  });

  const fetchMetadata = useCallback(async () => {
    try {
      const [statusRes, situationRes, typeRes, schedRes, catRes, userBankRes, catalogBankRes, cardRes] =
        await Promise.all([
          getTransactionStatuses({ page: 1, page_size: 20 }),
          getTransactionSituations({ page: 1, page_size: 20 }),
          getTransactionTypes({ page: 1, page_size: 20 }),
          getPaymentSchedulers({ page: 1, page_size: 20 }),
          getUserCategories({ page: 1, page_size: 30, active: true }),
          getUserBanks(),
          getCatalogBanks({ page: 1, page_size: 30 }),
          getUserCards({ page: 1, page_size: 30, active: true }),
        ]);

      const statusList = extractList(statusRes);
      const situationList = extractList(situationRes);
      const typeList = extractList(typeRes);
      const schedList = extractList(schedRes);
      const catList = extractList(catRes);
      const userBankList = extractList(userBankRes);
      const catalogBankList = extractList(catalogBankRes);
      const cardList = extractList(cardRes);

      // Map user banks to their corresponding catalog bank_id
      const enrichedBanks = userBankList.map((ub, idx) => {
        const matched = catalogBankList.find(
          (cb) => cb.name?.toLowerCase() === ub.name?.toLowerCase()
        );
        return {
          ...ub,
          resolvedBankId: ub.bank_id || matched?.bank_id || idx + 1,
        };
      });

      const availableBanks =
        enrichedBanks.length > 0
          ? enrichedBanks
          : catalogBankList.map((cb) => ({
              name: cb.name,
              nick_name: cb.name,
              resolvedBankId: cb.bank_id,
            }));

      setStatuses(statusList);
      setSituations(situationList);
      setTypes(typeList);
      setSchedulers(schedList);
      setCategories(catList);
      setBanks(availableBanks);
      setCards(cardList);

      setFormData((prev) => ({
        ...prev,
        category_name: prev.category_name || catList[0]?.name || '',
        transaction_status: prev.transaction_status || statusList[0]?.id || 1,
        transaction_situation: prev.transaction_situation || situationList[0]?.id || 1,
        transaction_type: prev.transaction_type || typeList[0]?.id || 1,
        bank: prev.bank || availableBanks[0]?.resolvedBankId || 1,
      }));
    } catch (err) {
      console.error(err);
    }
  }, []);

  const fetchTransactions = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const query = { page: 1, page_size: 20 };
      if (filters.transaction_status_id) query.transaction_status_id = Number(filters.transaction_status_id);
      if (filters.transaction_situation_id) query.transaction_situation_id = Number(filters.transaction_situation_id);
      if (filters.transaction_type_id) query.transaction_type_id = Number(filters.transaction_type_id);
      if (filters.category) query.category = Number(filters.category);
      if (filters.user_bank_card) query.user_bank_card = Number(filters.user_bank_card);

      const res = await getPaymentTransactions(query);
      setTransactions(extractList(res));
    } catch (err) {
      console.error(err);
      setError('Failed to load payment transactions.');
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchMetadata();
  }, [fetchMetadata]);

  useEffect(() => {
    fetchTransactions();
  }, [fetchTransactions]);

  const handleFilterChange = (field, value) => {
    setFilters((prev) => ({ ...prev, [field]: value }));
  };

  const handleResetFilters = () => {
    setFilters({
      transaction_status_id: '',
      transaction_situation_id: '',
      transaction_type_id: '',
      category: '',
      user_bank_card: '',
    });
  };

  const handleOpenModal = () => {
    setFeedback(null);
    setFormData({
      name: '',
      description: '',
      amount: '',
      pay_date: new Date().toISOString().slice(0, 10),
      category_name: categories[0]?.name || '',
      transaction_status: statuses[0]?.id || 1,
      transaction_situation: situations[0]?.id || 1,
      transaction_type: types[0]?.id || 1,
      bank: banks[0]?.resolvedBankId || 1,
      card: 0,
      scheduler: 0,
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
      const isoDate = new Date(`${formData.pay_date}T12:00:00Z`).toISOString();
      await createPaymentTransaction({
        name: formData.name.trim(),
        description: formData.description.trim(),
        amount: Math.round(Number(formData.amount)),
        pay_date: isoDate,
        category_name: formData.category_name,
        transaction_status: Number(formData.transaction_status),
        transaction_situation: Number(formData.transaction_situation),
        transaction_type: Number(formData.transaction_type),
        bank: Number(formData.bank),
        card: Number(formData.card) || 0,
        scheduler: Number(formData.scheduler) || 0,
      });
      setModalOpen(false);
      await fetchTransactions();
    } catch (err) {
      const msg = err.response?.data?.messages?.[0] || 'Failed to record payment transaction.';
      setFeedback({ type: 'error', message: msg });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <TransactionsView
      transactions={transactions}
      statuses={statuses}
      situations={situations}
      types={types}
      schedulers={schedulers}
      categories={categories}
      banks={banks}
      cards={cards}
      filters={filters}
      loading={loading}
      error={error}
      modalOpen={modalOpen}
      submitting={submitting}
      feedback={feedback}
      formData={formData}
      onFilterChange={handleFilterChange}
      onResetFilters={handleResetFilters}
      onOpenModal={handleOpenModal}
      onCloseModal={handleCloseModal}
      onFormChange={handleFormChange}
      onSubmit={handleSubmit}
    />
  );
};

export default Transactions;
