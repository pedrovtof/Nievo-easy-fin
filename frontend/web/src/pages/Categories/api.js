import { coreApi } from '../../services/api';

/**
 * Fetches the user's categories with their linked goal or parent hierarchy.
 *
 * @param {Object} params - Query parameters ({ page, page_size, active }).
 * @returns {Promise<Object>} Axios response containing paginated user categories.
 */
export const getUserCategories = (params = { page: 1, page_size: 30, active: true }) => {
  return coreApi.get('/api/public/v1/User/category', { params });
};

/**
 * Creates a new financial category linked exclusively to a goal or a parent category.
 *
 * @param {Object} data - Payload ({ name, description, goal, parent_category }).
 * @returns {Promise<Object>} Axios response confirming creation.
 */
export const createUserCategory = (data) => {
  return coreApi.post('/api/public/v1/User/category', data);
};

/**
 * Fetches the user's active goals to populate the category linkage selector.
 *
 * @param {Object} params - Query parameters ({ page, page_size, active }).
 * @returns {Promise<Object>} Axios response containing user goals.
 */
export const getUserGoals = (params = { page: 1, page_size: 30, active: true }) => {
  return coreApi.get('/api/public/v1/User/goal', { params });
};
