import { coreApi } from '../../services/api';

export const getDashboardData = () => {
  return coreApi.get('/dashboard');
};
