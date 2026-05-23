import api from '../../services/api';

// Step 1: Request a password reset PIN (sends email with token)
export const requestPasswordReset = (data) =>
  api.post('/api/public/v1/Authenticator/password-reset', data);

// Step 2: Confirm the reset using the PIN token received by email
export const confirmPasswordReset = (data) =>
  api.patch('/api/public/v1/Authenticator/password-reset', data);
