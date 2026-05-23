// This file intercepts the Axios requests when VITE_USE_MOCK is true.

let mockUsers = [
  { id: 1, name: 'Mock User', email: 'demo@nievo.com', password: 'password123' }
];

export const mockResponses = {
  loginSuccess: (user) => ({
    token: `mock-jwt-token-${user.id}`,
    user: { id: user.id, name: user.name, email: user.email }
  }),
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

export const getMockState = () => ({
  users: mockUsers.map(u => ({ email: u.email, password: u.password })),
  dashboard: {
    totalBalance: mockResponses.dashboard.data.totalBalance,
    income: mockResponses.dashboard.data.income,
    expenses: mockResponses.dashboard.data.expenses,
  }
});

export const setupMockAdapter = (apiClient) => {
  apiClient.interceptors.request.use((config) => {
    // If mock is not enabled, proceed normally
    if (import.meta.env.VITE_USE_MOCK !== 'true') return config;

    console.log(`[Mock Intercept] ${config.method.toUpperCase()} ${config.url}`);
    
    // We throw a special error with the mock response, which we'll catch in the response interceptor
    const error = new Error('mock-intercepted');
    
    if (config.url.includes('/auth/login') || (config.url.includes('/Authenticator/singin') && !config.url.includes('/Authenticator/singin-sso'))) {
      try {
        const body = typeof config.data === 'string' ? JSON.parse(config.data) : (config.data || {});
        const user = mockUsers.find(u => u.email === body.email && u.password === body.password);
        if (user) {
          error.mockData = { status: 200, data: mockResponses.loginSuccess(user) };
        } else {
          error.mockData = { 
            status: 401, 
            data: { success: false, messages: ["Invalid email or password. Check the Mock Guide."] } 
          };
        }
      } catch {
        error.mockData = { status: 401, data: { success: false, messages: ["Invalid login request."] } };
      }
    } else if (config.url.includes('/auth/register') || config.url.includes('/Users/singup')) {
      try {
        const body = JSON.parse(config.data);
        const exists = mockUsers.find(u => u.email === body.email);
        if (exists) {
          error.mockData = { status: 400, data: { success: false, messages: ["Email already in use."] } };
        } else {
          const newUser = { id: mockUsers.length + 1, name: body.fullName || 'New User', email: body.email, password: body.password };
          mockUsers.push(newUser);
          error.mockData = { status: 200, data: mockResponses.loginSuccess(newUser) };
        }
      } catch {
        error.mockData = { status: 400, data: { success: false, messages: ["Bad request."] } };
      }
    } else if (config.url.includes('/auth/change-password') || (config.url.includes('/Authenticator/password-reset') && config.method === 'patch')) {
      try {
        const body = JSON.parse(config.data);
        // Real API body: { email, pin_token, password }
        // Mock just validates pin_token is present and updates the password
        if (body.pin_token && body.password) {
          const user = mockUsers.find(u => u.email === body.email);
          if (user) {
            user.password = body.password;
            error.mockData = { status: 200, data: { success: true, message: "Password updated." } };
          } else {
            error.mockData = { status: 404, data: { success: false, messages: ["User not found"] } };
          }
        } else {
          error.mockData = { status: 401, data: { success: false, messages: ["Invalid or missing PIN token"] } };
        }
      } catch {
        error.mockData = { status: 400, data: { success: false, messages: ["Bad request"] } };
      }
    } else if (config.url.includes('/auth/forgot-password') || (config.url.includes('/Authenticator/password-reset') && config.method === 'post')) {
      try {
        typeof config.data === 'string' ? JSON.parse(config.data) : (config.data || {});
        // In a real app we don't usually leak if the email exists, we just return 200.
        error.mockData = { status: 200, data: { success: true, message: "Reset link sent." } };
      } catch {
        error.mockData = { status: 400, data: { success: false, messages: ["Bad request."] } };
      }
    } else if (config.url.includes('/dashboard')) {
      error.mockData = { status: 200, data: mockResponses.dashboard };
    } else {
      error.mockData = { status: 200, data: { success: true } };
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
