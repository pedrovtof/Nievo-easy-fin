import api from '../../services/api';

// Step 1: Create user (sends verification token via email, user status = INVALID)
export const createUser = (data) => api.post('/api/public/v1/Users/singup', data);

// Step 2: Validate email with the PIN token received in the inbox
export const validateEmailToken = (data) => api.post('/api/public/v1/Authenticator/validate:email', data);

// SSO signup (unchanged)
export const createUserSSO = (data) => api.post('/api/public/v1/Users/singup-sso', data);

// Fetch terms of use content (returns { content, version, ... })
export const getAcceptTerms = () => api.get('/api/public/v1/Authenticator/accept-terms:singup');

// Resend the email verification PIN to the given address
export const sendValidateEmail = (data) => api.post('/api/public/v1/Authenticator/send-validate:email', data);
