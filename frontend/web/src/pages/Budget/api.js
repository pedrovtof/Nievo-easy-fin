import api from '../../services/api';

/**
 * Fetches paginated user financial goals and budgets.
 *
 * @param {Object} params - Query parameters ({ page, page_size, active }).
 * @returns {Promise<Object>} Axios response containing user goals.
 */
export const getUserGoals = (params = { page: 1, page_size: 20, active: true }) => {
  return api.get('/api/public/v1/User/goal', { params });
};

/**
 * Creates a new financial goal or spending limit for the authenticated user.
 *
 * @param {Object} data - Payload ({ name, description, amount, is_percent, expire_at }).
 * @returns {Promise<Object>} Axios response confirming creation.
 */
export const createUserGoal = (data) => {
  return api.post('/api/public/v1/User/goal', data);
};
