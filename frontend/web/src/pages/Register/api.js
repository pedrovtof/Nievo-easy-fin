import api from '../../services/api';

export const createUser = (data) => api.post('/auth/register', data);
export const createUserSSO = (data) => api.post('/auth/sso/register', data);
