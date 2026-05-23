import api from '../../services/api';

export const requestPasswordReset = (data) => api.post('/auth/forgot-password', data);
