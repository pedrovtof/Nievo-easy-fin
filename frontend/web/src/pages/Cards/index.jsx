import React, { useState, useEffect, useCallback } from 'react';
import CardsView from './View';
import {
  getUserCards,
  createUserCard,
  getCatalogBankCards,
  getCatalogBanks,
  getCardTypes,
  getCardFlags,
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
 * Cards Page Controller
 * Orchestrates user wallet cards, card catalog products, and card metadata (types and flags).
 *
 * @returns {JSX.Element} The rendered Cards page controller.
 */
const Cards = () => {
  const [userCards, setUserCards] = useState([]);
  const [catalogCards, setCatalogCards] = useState([]);
  const [banks, setBanks] = useState([]);
  const [cardTypes, setCardTypes] = useState([]);
  const [cardFlags, setCardFlags] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [filters, setFilters] = useState({
    bank_id: '',
    flag: '',
    active: true,
  });

  const [modalOpen, setModalOpen] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [feedback, setFeedback] = useState(null);
  const [formData, setFormData] = useState({
    bank_id: '',
    card_id: '',
    nickname: '',
    expire_at: '2029-12-31',
  });

  const fetchMetadataAndCatalog = useCallback(async () => {
    try {
      const [banksRes, catalogRes, typesRes, flagsRes] = await Promise.all([
        getCatalogBanks({ page: 1, page_size: 30 }),
        getCatalogBankCards({ page: 1, page_size: 30 }),
        getCardTypes({ page: 1, page_size: 20 }),
        getCardFlags({ page: 1, page_size: 20 }),
      ]);
      const bankList = extractList(banksRes);
      const cardList = extractList(catalogRes);
      setBanks(bankList);
      setCatalogCards(cardList);
      setCardTypes(extractList(typesRes));
      setCardFlags(extractList(flagsRes));

      if (bankList.length > 0 || cardList.length > 0) {
        setFormData((prev) => ({
          ...prev,
          bank_id: prev.bank_id || bankList[0]?.bank_id || '',
          card_id: prev.card_id || cardList[0]?.id || '',
        }));
      }
    } catch (err) {
      console.error(err);
    }
  }, []);

  const fetchWalletCards = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const query = {
        page: 1,
        page_size: 20,
        active: filters.active,
      };
      if (filters.bank_id) query.bank_id = Number(filters.bank_id);
      if (filters.flag) query.flag = filters.flag;

      const res = await getUserCards(query);
      setUserCards(extractList(res));
    } catch (err) {
      console.error(err);
      setError('Failed to load wallet cards.');
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchMetadataAndCatalog();
  }, [fetchMetadataAndCatalog]);

  useEffect(() => {
    fetchWalletCards();
  }, [fetchWalletCards]);

  const handleFilterChange = (field, value) => {
    setFilters((prev) => ({ ...prev, [field]: value }));
  };

  const handleOpenModal = (catalogItem = null) => {
    setFeedback(null);
    if (catalogItem) {
      const matchedBank = banks.find(
        (b) => b.name?.toLowerCase() === catalogItem.bank?.toLowerCase()
      );
      setFormData({
        bank_id: matchedBank?.bank_id || banks[0]?.bank_id || 1,
        card_id: catalogItem.id,
        nickname: catalogItem.card_name || '',
        expire_at: '2029-12-31',
      });
    } else {
      setFormData({
        bank_id: banks[0]?.bank_id || 1,
        card_id: catalogCards[0]?.id || 1,
        nickname: '',
        expire_at: '2029-12-31',
      });
    }
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
      await createUserCard({
        bank_id: Number(formData.bank_id),
        card_id: Number(formData.card_id),
        nickname: formData.nickname.trim(),
        expire_at: isoExpire,
      });
      setModalOpen(false);
      await fetchWalletCards();
    } catch (err) {
      const msg = err.response?.data?.messages?.[0] || 'Failed to register card to your wallet.';
      setFeedback({ type: 'error', message: msg });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <CardsView
      userCards={userCards}
      catalogCards={catalogCards}
      banks={banks}
      cardTypes={cardTypes}
      cardFlags={cardFlags}
      filters={filters}
      loading={loading}
      error={error}
      modalOpen={modalOpen}
      submitting={submitting}
      feedback={feedback}
      formData={formData}
      onFilterChange={handleFilterChange}
      onOpenModal={handleOpenModal}
      onCloseModal={handleCloseModal}
      onFormChange={handleFormChange}
      onSubmit={handleSubmit}
    />
  );
};

export default Cards;
