import api from '../../services/api';

/**
 * Fetches paginated payment transactions with optional filters.
 *
 * @param {Object} params - Query parameters ({ page, page_size, user_bank_card, scheduler_exists, transaction_status_id, transaction_type_id, category, pay_date_start, pay_date_end }).
 * @returns {Promise<Object>} Axios response containing paginated transactions.
 */
export const getPaymentTransactions = (params = { page: 1, page_size: 20 }) => {
  return api.get('/api/public/v1/Payment', { params });
};

/**
 * Creates a new user payment transaction.
 *
 * @param {Object} data - Payload ({ name, description, amount, pay_date, category_name, transaction_status, transaction_type, bank, card, scheduler }).
 * @returns {Promise<Object>} Axios response confirming creation.
 */
export const createPaymentTransaction = (data) => {
  return api.post('/api/public/v1/Payment', data);
};

/**
 * Fetches transaction status metadata options.
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing statuses.
 */
export const getTransactionStatuses = (params = { page: 1, page_size: 20 }) => {
  return api.get('/api/public/v1/Payment/status', { params });
};

/**
 * Fetches transaction type metadata options (Expense, Income, Transfer).
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing types.
 */
export const getTransactionTypes = (params = { page: 1, page_size: 20 }) => {
  return api.get('/api/public/v1/Payment/type', { params });
};

/**
 * Fetches recurrence/scheduler type options.
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing scheduler types.
 */
export const getPaymentSchedulers = (params = { page: 1, page_size: 20 }) => {
  return api.get('/api/public/v1/Payment/scheduler', { params });
};

/**
 * Fetches the user's active categories to populate transaction filters and creation form.
 *
 * @param {Object} params - Pagination parameters ({ page, page_size, active }).
 * @returns {Promise<Object>} Axios response containing user categories.
 */
export const getUserCategories = (params = { page: 1, page_size: 30, active: true }) => {
  return api.get('/api/public/v1/User/category', { params });
};

/**
 * Fetches the user's linked bank accounts.
 *
 * @returns {Promise<Object>} Axios response containing user banks.
 */
export const getUserBanks = () => {
  return api.get('/api/public/v1/User/banks');
};

/**
 * Fetches the public bank catalog to resolve bank IDs when creating transactions.
 *
 * @param {Object} params - Pagination parameters ({ page, page_size }).
 * @returns {Promise<Object>} Axios response containing catalog banks.
 */
export const getCatalogBanks = (params = { page: 1, page_size: 30 }) => {
  return api.get('/api/public/v1/Bank', { params });
};

/**
 * Fetches the user's registered cards.
 *
 * @param {Object} params - Pagination parameters ({ page, page_size, active }).
 * @returns {Promise<Object>} Axios response containing user cards.
 */
export const getUserCards = (params = { page: 1, page_size: 30, active: true }) => {
  return api.get('/api/public/v1/User/bank-card', { params });
};
