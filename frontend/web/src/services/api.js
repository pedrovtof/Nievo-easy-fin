import axios from 'axios';
import { setupMockAdapter } from './mockData';

const baseAuthUrl =
  import.meta.env.VITE_BASE_AUTH_URL ||
  import.meta.env.VITE_API_AUTH_URL ||
  import.meta.env.VITE_API_BASE_URL ||
  'http://localhost:5090';

const baseCoreUrl =
  import.meta.env.VITE_BASE_CORE_URL ||
  import.meta.env.VITE_API_CORE_URL ||
  'http://localhost:5203';

const apiTimeout = Number(import.meta.env.VITE_TIMEOUT_API) || 10000;

/**
 * Attaches the JWT Bearer token from localStorage to outgoing Axios requests.
 *
 * @param {Object} instance - Axios client instance.
 */
const configureInterceptors = (instance) => {
  instance.interceptors.request.use((config) => {
    const token = localStorage.getItem('auth_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });

  if (import.meta.env.VITE_USE_MOCK === 'true') {
    setupMockAdapter(instance);
  }
};

/**
 * Auth API Client instance (NievoEasyFin.Auth - default port 5090).
 * Handles login, signup, email validation, terms, and password reset.
 */
export const authApi = axios.create({
  baseURL: baseAuthUrl,
  timeout: apiTimeout,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Core API Client instance (NievoEasyFin.Core - default port 5203).
 * Handles banks, cards, goals, categories, and payment transactions.
 */
export const coreApi = axios.create({
  baseURL: baseCoreUrl,
  timeout: apiTimeout,
  headers: {
    'Content-Type': 'application/json',
  },
});

configureInterceptors(authApi);
configureInterceptors(coreApi);

if (import.meta.env.VITE_USE_MOCK === 'true') {
  console.log('Mock Data Adapter enabled for Auth and Core API clients.');
}

export default authApi;
