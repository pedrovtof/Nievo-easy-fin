import api from '../../services/api';

export const loginUser = (data) => api.post('/auth/login', data);
export const loginUserSSO = (data) => api.post('/auth/sso', data);
