import api from '../../services/api';

export const createUser = (data) => api.post('/api/public/v1/Users/singup', data);
export const createUserSSO = (data) => api.post('/api/public/v1/Users/singup-sso', data);
