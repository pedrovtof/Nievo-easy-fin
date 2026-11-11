import { coreApi } from '../../services/api';

/**
 * Fetches the user's registered bank cards from the wallet.
 *
 * @param {Object} params - Query parameters ({ page, page_size, bank_id, active, flag }).
 * @returns {Promise<Object>} Axios response containing paginated user cards.
 */
export const getUserCards = (params = { page: 1, page_size: 20, active: true }) => {
  return coreApi.get('/api/public/v1/User/bank-card', { params });
};

/**
 * Registers a new bank card to the user's wallet.
 *
 * @param {Object} data - Payload ({ bank_id, card_id, nickname, expire_at }).
 * @returns {Promise<Object>} Axios response confirming creation.
 */
export const createUserCard = (data) => {
  return coreApi.post('/api/public/v1/User/bank-card', data);
};

/**
 * Fetches the public catalog of available bank card products.
 *
 * @param {Object} params - Query parameters ({ page, page_size, bank, card_type, flag }).
 * @returns {Promise<Object>} Axios response containing catalog bank cards.
 */
export const getCatalogBankCards = (params = { page: 1, page_size: 30 }) => {
  return coreApi.get('/api/public/v1/Bank/card', { params });
};

/**
 * Fetches the public catalog of banking institutions.
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing banks.
 */
export const getCatalogBanks = (params = { page: 1, page_size: 30 }) => {
  return coreApi.get('/api/public/v1/Bank', { params });
};

/**
 * Fetches supported card types (Credit, Debit, Multiple).
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing card types.
 */
export const getCardTypes = (params = { page: 1, page_size: 20 }) => {
  return coreApi.get('/api/public/v1/Card/type', { params });
};

/**
 * Fetches supported card flags/networks (Mastercard, Visa, Elo, Amex).
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing card flags.
 */
export const getCardFlags = (params = { page: 1, page_size: 20 }) => {
  return coreApi.get('/api/public/v1/Card/flag', { params });
};
