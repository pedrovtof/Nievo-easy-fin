import axios from 'axios';
import { setupMockAdapter } from './mockData';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:3000/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Setup request interceptor to add token
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// If VITE_USE_MOCK is true, setup the mock adapter
if (import.meta.env.VITE_USE_MOCK === 'true') {
  setupMockAdapter(api);
  console.log('Mock Data Adapter enabled.');
}

export default api;
