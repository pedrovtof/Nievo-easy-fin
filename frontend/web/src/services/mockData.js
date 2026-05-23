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
  userEmail: mockUsers[0].email,
  userPassword: mockUsers[0].password,
  totalBalance: mockResponses.dashboard.data.totalBalance,
  income: mockResponses.dashboard.data.income,
  expenses: mockResponses.dashboard.data.expenses,
});

export const updateMockState = (updates) => {
  if (updates.userEmail !== undefined) mockUsers[0].email = updates.userEmail;
  if (updates.userPassword !== undefined) mockUsers[0].password = updates.userPassword;
  if (updates.totalBalance !== undefined) mockResponses.dashboard.data.totalBalance = Number(updates.totalBalance);
  if (updates.income !== undefined) mockResponses.dashboard.data.income = Number(updates.income);
  if (updates.expenses !== undefined) mockResponses.dashboard.data.expenses = Number(updates.expenses);
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
        const user = mockUsers.find(u => u.email === body.email && u.password === body.password);
        if (user) {
          error.mockData = { status: 200, data: mockResponses.loginSuccess(user) };
        } else {
          error.mockData = { 
            status: 401, 
            data: { success: false, messages: ["Invalid email or password. Check the Mock Guide."] } 
          };
        }
      } catch (e) {
        error.mockData = { status: 200, data: mockResponses.loginSuccess(mockUsers[0]) };
      }
    } else if (config.url.includes('/auth/register')) {
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
      } catch (e) {
        error.mockData = { status: 400, data: { success: false, messages: ["Bad request."] } };
      }
    } else if (config.url.includes('/auth/change-password')) {
      try {
        const body = JSON.parse(config.data);
        // Assuming body contains { email, oldPassword, newPassword }
        const user = mockUsers.find(u => u.email === body.email && u.password === body.oldPassword);
        if (user) {
          user.password = body.newPassword;
          error.mockData = { status: 200, data: { success: true, message: "Password updated." } };
        } else {
          error.mockData = { status: 401, data: { success: false, messages: ["Invalid old password or email."] } };
        }
      } catch (e) {
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
