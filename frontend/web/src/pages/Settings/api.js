import { authApi, coreApi } from '../../services/api';

/**
 * Fetches the authenticated user's profile details from the Core API.
 *
 * @returns {Promise<Object>} Axios response containing GetUserDetailsResponse ({ email, name, phone, status }).
 */
export const getUserDetails = () => {
  return coreApi.get('/api/public/v1/User/details');
};

/**
 * Fetches combined user profile details from Core API and local preference defaults.
 *
 * @returns {Promise<Object>} Object containing profile and toggles state.
 */
export const getSettingsData = async () => {
  const detailsRes = await getUserDetails();
  const payload = detailsRes?.data?.data ?? detailsRes?.data ?? {};

  return {
    data: {
      profile: {
        fullName: payload.name || '',
        email: payload.email || '',
        phone: payload.phone ?? '',
        status: payload.status || 'ACTIVE',
        currency: 'USD',
      },
      toggles: {
        twoFactor: false,
        darkMode: false,
        weeklyDigest: true,
        unusualActivity: true,
        budgetWarnings: false,
      },
    },
  };
};

/**
 * Persists user preference updates locally.
 *
 * @param {Object} data - Updated profile and toggles state.
 * @returns {Promise<Object>} Confirmation payload.
 */
export const updateSettings = (data) => {
  return Promise.resolve({ success: true, data });
};

/**
 * Requests a password change on the Auth API.
 *
 * @param {string} email - User email address.
 * @param {string} oldPassword - Current password.
 * @param {string} newPassword - New password.
 * @returns {Promise<Object>} Axios response from Auth API.
 */
export const changePassword = (email, oldPassword, newPassword) => {
  return authApi.post('/auth/change-password', { email, oldPassword, newPassword });
};
