import { authApi } from '../../services/api';

// Step 1: Request a password reset PIN (sends email with token)
export const requestPasswordReset = (data) =>
  authApi.post('/api/public/v1/Users/password-reset', data);

// Step 2: Confirm the reset using the PIN token received by email
export const confirmPasswordReset = (data) =>
  authApi.patch('/api/public/v1/Users/password-reset', data);
