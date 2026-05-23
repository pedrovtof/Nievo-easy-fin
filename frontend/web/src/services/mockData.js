// This file intercepts the Axios requests when VITE_USE_MOCK is true.

export const mockResponses = {
  login: {
    data: {
      token: "mock-jwt-token-12345",
      user: {
        id: 1,
        name: "Mock User",
        email: "user@mock.com"
      }
    }
  },
  dashboard: {
    data: {
      totalBalance: 15420.50,
      income: 5000.00,
      expenses: 3200.25,
      recentTransactions: [
        { id: 1, description: "Grocery Store", amount: -150.00, date: "2026-05-20", category: "Food" },
        { id: 2, description: "Salary", amount: 5000.00, date: "2026-05-15", category: "Income" }
      ]
    }
  }
};

export const setupMockAdapter = (apiClient) => {
  apiClient.interceptors.request.use((config) => {
    // If mock is not enabled, proceed normally
    if (import.meta.env.VITE_USE_MOCK !== 'true') return config;

    console.log(`[Mock Intercept] ${config.method.toUpperCase()} ${config.url}`);
    
    // We throw a special error with the mock response, which we'll catch in the response interceptor
    const error = new Error('mock-intercepted');
    
    if (config.url.includes('/auth/login')) {
      try {
        const body = JSON.parse(config.data);
        if (body.email === 'demo@nievo.com' && body.password === 'password123') {
          error.mockData = { status: 200, data: mockResponses.login };
        } else {
          error.mockData = { 
            status: 401, 
            data: { success: false, messages: ["Invalid email or password. Use demo@nievo.com / password123"] } 
          };
        }
      } catch (e) {
        // If no body or parse error, just accept it for simplicity or SSO
        error.mockData = { status: 200, data: mockResponses.login };
      }
    } else if (config.url.includes('/dashboard')) {
      error.mockData = { status: 200, data: mockResponses.dashboard };
    } else {
      error.mockData = { status: 200, data: {} };
    }
    
    return Promise.reject(error);
  });

  apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.message === 'mock-intercepted') {
        return Promise.resolve(error.mockData);
      }
      return Promise.reject(error);
    }
  );
};
