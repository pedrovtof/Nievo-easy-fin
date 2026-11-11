import api from '../../services/api';

/**
 * Fetches the public catalog of supported banking institutions.
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing paginated banks.
 */
export const getCatalogBanks = (params = { page: 1, page_size: 20 }) => {
  return api.get('/api/public/v1/Bank', { params });
};

/**
 * Fetches the bank accounts linked to the currently authenticated user.
 *
 * @returns {Promise<Object>} Axios response containing the user's linked bank accounts.
 */
export const getUserBanks = () => {
  return api.get('/api/public/v1/User/banks');
};

/**
 * Links a new bank account from the catalog to the authenticated user's profile.
 *
 * @param {Object} data - Payload ({ nickname, bank_type, bank_name }).
 * @returns {Promise<Object>} Axios response confirming creation.
 */
export const createUserBank = (data) => {
  return api.post('/api/public/v1/User/banks', data);
};
