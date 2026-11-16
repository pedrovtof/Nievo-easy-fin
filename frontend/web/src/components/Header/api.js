import { coreApi } from '../../services/api';

/**
 * Fetches the authenticated user's profile details from the Core API.
 *
 * @returns {Promise<Object>} Axios response containing GetUserDetailsResponse ({ email, name, phone, status }).
 */
export const getUserDetails = () => {
  return coreApi.get('/api/public/v1/User/details');
};

export const headerApiConfig = {
  getUserDetails,
};
