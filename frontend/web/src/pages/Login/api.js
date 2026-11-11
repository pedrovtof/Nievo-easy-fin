import { authApi } from '../../services/api';

/**
 * Authenticates a user using standard email and password credentials.
 *
 * @param {Object} data - Login payload ({ email, password }).
 * @returns {Promise<Object>} Axios response from NievoEasyFin.Auth.
 */
export const loginUser = (data) => authApi.post('/api/public/v1/Authenticator/singin', data);

/**
 * Authenticates a user using a Google SSO provider token.
 *
 * @param {Object} data - SSO payload ({ provider_name, provider_access_token }).
 * @returns {Promise<Object>} Axios response from NievoEasyFin.Auth.
 */
export const loginUserSSO = (data) => authApi.post('/api/public/v1/Authenticator/singin-sso', data);
