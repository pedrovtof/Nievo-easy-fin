import api from '../../services/api';

// Fetch the current terms of use content for signup
export const getAcceptTerms = () => api.get('/api/public/v1/Authenticator/accept-terms:singup');
