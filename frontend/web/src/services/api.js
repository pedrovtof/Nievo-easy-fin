import axios from 'axios';

const API_CONFIG = {
    BASE_URL: import.meta.env.VITE_BASE_URL_API || 'http://localhost:5090/api/',
    TIMEOUT: import.meta.env.VITE_TIMEOUT_API || 10000,
};

// ─── Shared response interceptor ─────────────────────────────────────────────

const applyResponseInterceptor = (instance) => {
    instance.interceptors.response.use(
        (response) => {
            const { status, message } = response.data ?? {};
            if (status && message && status === 'error') {
                console.error(`API Error: ${message}`);
            }
            return response;
        },
        (error) => Promise.reject(error)
    );
};

// ─── Public API (rotas sem autenticação) ─────────────────────────────────────

export const publicApi = axios.create({
    baseURL: `${API_CONFIG.BASE_URL}public/`,
    timeout: API_CONFIG.TIMEOUT,
    headers: { 'Content-Type': 'application/json' },
});

applyResponseInterceptor(publicApi);

// ─── Private API (rotas autenticadas) ────────────────────────────────────────

export const privateApi = axios.create({
    baseURL: `${API_CONFIG.BASE_URL}private/`,
    timeout: API_CONFIG.TIMEOUT,
    headers: { 'Content-Type': 'application/json' },
});

// Injeta o token automaticamente nas requisições privadas
privateApi.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

applyResponseInterceptor(privateApi);

// ─── Default export (mantém compatibilidade com imports existentes) ───────────

export default publicApi;

// ─── Users ───────────────────────────────────────────────────────────────────

/**
 * Cria um novo usuário com email e senha.
 * @param {{ name: string, email: string, password: string }} userData
 * @returns {Promise<import('axios').AxiosResponse>}
 */
export const createUser = (userData) => publicApi.post('v1/Users/singup', userData);

/**
 * Cria um novo usuário via SSO (ex: Google).
 * @param {{ provider_name: string, provider_access_token: string }} ssoData
 * @returns {Promise<import('axios').AxiosResponse>}
 */
export const createUserSSO = (ssoData) => publicApi.post('v1/Users/singup-sso', ssoData);

// ─── Auth ─────────────────────────────────────────────────────────────────────

/**
 * Autentica um usuário com email e senha.
 * @param {{ email: string, password: string }} credentials
 * @returns {Promise<import('axios').AxiosResponse>}
 */
export const loginUser = (credentials) => publicApi.post('v1/Authenticator/singin', credentials);

/**
 * Autentica um usuário via SSO (ex: Google).
 * @param {{ provider_name: string, provider_access_token: string }} ssoData
 * @returns {Promise<import('axios').AxiosResponse>}
 */
export const loginUserSSO = (ssoData) => publicApi.post('v1/Authenticator/singin-sso', ssoData);

