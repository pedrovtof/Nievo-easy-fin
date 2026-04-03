import axios from 'axios';

const API_CONFIG = {
    BASE_URL: import.meta.env.VITE_BASE_URL_API || 'http://localhost:5090/api/',
    TIMEOUT: import.meta.env.VITE_TIMEOUT_API || 10000,
};

const api = axios.create({
    baseURL: API_CONFIG.BASE_URL,
    timeout: API_CONFIG.TIMEOUT,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request interceptor
api.interceptors.request.use(
    (config) => {
        // You can add auth tokens here if needed
        // const token = localStorage.getItem('token');
        // if (token) {
        //   config.headers.Authorization = `Bearer ${token}`;
        // }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Response interceptor to handle standard message/status format
api.interceptors.response.use(
    (response) => {
        // Use the standard response format if present
        const { status, message, data } = response.data;

        // Check if the response matches your standard structure
        if (status && message) {
            // You can handle specific statuses here globally (e.g. show toast)
            if (status === 'error') {
                console.error(`API Error: ${message}`);
                // Optionally reject here if you want 'error' status to be caught
                // return Promise.reject(new Error(message));
            }
        }

        // Return the response.data directly for easier access, or the full response
        // returning full response to access status/headers if needed, 
        // but usually response.data is what we want.
        // Let's return the full response object but attach helper properties if needed.
        return response;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default api;

// ─── Users ───────────────────────────────────────────────────────────────────

/**
 * Cria um novo usuário via OAuth do Google.
 * @param {{ email: string, name: string, picture: string, sub: string }} userData
 * @returns {Promise<import('axios').AxiosResponse>}
 */
export const createUser = (userData) => api.post('v1/Users/', userData);
