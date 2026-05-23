import api from '../../services/api';

export const loginUser = (data) => api.post('/api/public/v1/Authenticator/singin', data);
export const loginUserSSO = (data) => api.post('/api/public/v1/Authenticator/singin-sso', data);

