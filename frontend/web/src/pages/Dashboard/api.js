import api from '../../services/api';

export const getDashboardData = () => {
  return api.get('/dashboard');
};
