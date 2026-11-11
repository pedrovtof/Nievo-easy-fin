// This file intercepts the Axios requests when VITE_USE_MOCK is true.

let mockUsers = [
  { id: 1, name: 'Mock User', email: 'demo@nievo.com', password: 'password123' }
];

let mockCatalogBanks = [
  { bank_id: 1, name: 'Itaú Unibanco', bank_type: 1, bank_type_name: 'Checking / Retail', bank_description: 'Traditional retail and digital banking' },
  { bank_id: 2, name: 'Nubank', bank_type: 2, bank_type_name: 'Digital Bank', bank_description: 'Modern digital payment institution' },
  { bank_id: 3, name: 'Banco Bradesco', bank_type: 1, bank_type_name: 'Checking / Retail', bank_description: 'Full-service commercial bank' },
  { bank_id: 4, name: 'Inter', bank_type: 2, bank_type_name: 'Digital Bank', bank_description: 'Digital banking and investment platform' },
];

let mockCardTypes = [
  { id: 1, name: 'Credit', description: 'Credit card with monthly billing cycle', active: true },
  { id: 2, name: 'Debit', description: 'Direct debit from checking account', active: true },
  { id: 3, name: 'Multiple', description: 'Combined credit and debit functionality', active: true },
];

let mockCardFlags = [
  { name: 'Mastercard', description: 'Mastercard International Network' },
  { name: 'Visa', description: 'Visa Global Payment Network' },
  { name: 'Elo', description: 'Elo Domestic & International Network' },
  { name: 'Amex', description: 'American Express Network' },
];

let mockCatalogBankCards = [
  { id: 1, bank: 'Itaú Unibanco', bank_id: 1, card_type: 'Multiple', card_type_id: 3, card_name: 'Itaú Personnalité Black', card_flag: 'Mastercard' },
  { id: 2, bank: 'Nubank', bank_id: 2, card_type: 'Multiple', card_type_id: 3, card_name: 'Nubank Ultravioleta', card_flag: 'Mastercard' },
  { id: 3, bank: 'Banco Bradesco', bank_id: 3, card_type: 'Credit', card_type_id: 1, card_name: 'Bradesco Visa Infinite', card_flag: 'Visa' },
  { id: 4, bank: 'Inter', bank_id: 4, card_type: 'Multiple', card_type_id: 3, card_name: 'Inter Black', card_flag: 'Mastercard' },
];

let mockUserBanks = [
  { id: 1, bank_id: 1, name: 'Itaú Unibanco', nick_name: 'Main Checking Account', bank_type: 'Checking / Retail' },
  { id: 2, bank_id: 2, name: 'Nubank', nick_name: 'Daily Reserve', bank_type: 'Digital Bank' },
];

let mockUserBankCards = [
  {
    user_bank_card_id: 1,
    user_bank_card_nickname: 'Personal Black Card',
    active: true,
    expired_at: '2029-12-31T00:00:00Z',
    bank_name: 'Itaú Unibanco',
    bank_id: 1,
    bank_card_name: 'Itaú Personnalité Black',
    bank_card_type: 'Multiple',
    bank_card_flag: 'Mastercard',
  },
  {
    user_bank_card_id: 2,
    user_bank_card_nickname: 'Virtual Subscriptions Card',
    active: true,
    expired_at: '2028-08-15T00:00:00Z',
    bank_name: 'Nubank',
    bank_id: 2,
    bank_card_name: 'Nubank Ultravioleta',
    bank_card_type: 'Multiple',
    bank_card_flag: 'Mastercard',
  },
];

let mockUserGoals = [
  {
    id: 1,
    name: 'Emergency Reserve',
    description: 'Build 6 months of essential family living expenses',
    amount: 15000,
    is_percent: false,
    active: true,
    expire_at: '2027-12-31T00:00:00Z',
    created_at: '2026-01-10T10:00:00Z',
    updated_at: '2026-01-10T10:00:00Z',
  },
  {
    id: 2,
    name: 'Monthly Essential Cap',
    description: 'Keep fixed household costs within target budget',
    amount: 2400,
    is_percent: false,
    active: true,
    expire_at: '2027-06-30T00:00:00Z',
    created_at: '2026-02-01T10:00:00Z',
    updated_at: '2026-02-01T10:00:00Z',
  },
  {
    id: 3,
    name: 'Savings Allocation Rate',
    description: 'Percentage of monthly income dedicated to investments',
    amount: 20,
    is_percent: true,
    active: true,
    expire_at: '2027-12-31T00:00:00Z',
    created_at: '2026-03-01T10:00:00Z',
    updated_at: '2026-03-01T10:00:00Z',
  },
];

let mockUserCategories = [
  {
    id: '1',
    name: 'Groceries & Market',
    description: 'Supermarket and household essentials',
    active: true,
    parent_category: null,
    goal_name: 'Monthly Essential Cap',
    goal_description: 'Keep fixed household costs within target budget',
    created_at: '2026-02-05T12:00:00Z',
    updated_at: '2026-02-05T12:00:00Z',
  },
  {
    id: '2',
    name: 'Utilities & Bills',
    description: 'Electricity, water, internet, and condo fees',
    active: true,
    parent_category: null,
    goal_name: 'Monthly Essential Cap',
    goal_description: 'Keep fixed household costs within target budget',
    created_at: '2026-02-06T12:00:00Z',
    updated_at: '2026-02-06T12:00:00Z',
  },
  {
    id: '3',
    name: 'Salary & Freelance',
    description: 'Primary and secondary household earnings',
    active: true,
    parent_category: null,
    goal_name: 'Emergency Reserve',
    goal_description: 'Build 6 months of essential family living expenses',
    created_at: '2026-02-07T12:00:00Z',
    updated_at: '2026-02-07T12:00:00Z',
  },
  {
    id: '4',
    name: 'Organic Produce',
    description: 'Weekly farmer market purchases',
    active: true,
    parent_category: 1,
    goal_name: '',
    goal_description: '',
    created_at: '2026-02-10T12:00:00Z',
    updated_at: '2026-02-10T12:00:00Z',
  },
];

let mockTransactionStatuses = [
  { id: 1, name: 'Completed', description: 'Payment settled and confirmed', active: true },
  { id: 2, name: 'Pending', description: 'Scheduled or awaiting clearing', active: true },
  { id: 3, name: 'Cancelled', description: 'Transaction voided or refunded', active: true },
];

let mockTransactionTypes = [
  { id: 1, name: 'Expense', description: 'Outflow payment or purchase', active: true },
  { id: 2, name: 'Income', description: 'Inflow deposit or earnings', active: true },
  { id: 3, name: 'Transfer', description: 'Internal movement between accounts', active: true },
];

let mockSchedulers = [
  { id: 1, name: 'One-Time', description: 'Single occurrence transaction', active: true, cron_tab: '' },
  { id: 2, name: 'Monthly', description: 'Repeats every month on the pay date', active: true, cron_tab: '0 0 1 * *' },
  { id: 3, name: 'Weekly', description: 'Repeats every week', active: true, cron_tab: '0 0 * * 1' },
];

let mockPaymentTransactions = [
  {
    id: 'a1b2c3d4-0001-4000-8000-000000000001',
    name: 'Whole Foods Market',
    description: 'Weekly family grocery shopping',
    amount: 285,
    cron_tab: '',
    status: 'Completed',
    type: 'Expense',
    category: 'Groceries & Market',
    pay_date: '2026-11-10T14:30:00Z',
    user_card_name: 'Personal Black Card',
    user_bank_nickname: 'Main Checking Account',
    bank_name: 'Itaú Unibanco',
    created_at: '2026-11-10T14:30:00Z',
  },
  {
    id: 'a1b2c3d4-0002-4000-8000-000000000002',
    name: 'Monthly Salary Deposit',
    description: 'Primary software engineering compensation',
    amount: 5400,
    cron_tab: '0 0 1 * *',
    status: 'Completed',
    type: 'Income',
    category: 'Salary & Freelance',
    pay_date: '2026-11-05T09:00:00Z',
    user_card_name: '',
    user_bank_nickname: 'Main Checking Account',
    bank_name: 'Itaú Unibanco',
    created_at: '2026-11-05T09:00:00Z',
  },
  {
    id: 'a1b2c3d4-0003-4000-8000-000000000003',
    name: 'City Power & Fiber Internet',
    description: 'Monthly electricity and gigabit internet bill',
    amount: 195,
    cron_tab: '0 0 1 * *',
    status: 'Pending',
    type: 'Expense',
    category: 'Utilities & Bills',
    pay_date: '2026-11-15T10:00:00Z',
    user_card_name: 'Virtual Subscriptions Card',
    user_bank_nickname: 'Daily Reserve',
    bank_name: 'Nubank',
    created_at: '2026-11-08T10:00:00Z',
  },
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

/**
 * Helper to wrap paginated responses in the exact Core API format:
 * { success: true, data: { page, page_size, records, items } }
 */
const wrapPaginated = (items, page = 1, pageSize = 20) => ({
  success: true,
  data: {
    page: Number(page) || 1,
    page_size: Number(pageSize) || 20,
    records: items.length,
    items,
  }
});

/**
 * Helper to parse JSON body safely from Axios request config.
 */
const parseBody = (data) => {
  if (!data) return {};
  if (typeof data === 'string') {
    try {
      return JSON.parse(data);
    } catch {
      return {};
    }
  }
  return data;
};

export const getMockState = () => ({
  users: mockUsers.map(u => ({ email: u.email, password: u.password })),
  dashboard: {
    totalBalance: mockResponses.dashboard.data.totalBalance,
    income: mockResponses.dashboard.data.income,
    expenses: mockResponses.dashboard.data.expenses,
  },
  counts: {
    userBanks: mockUserBanks.length,
    userCards: mockUserBankCards.length,
    goals: mockUserGoals.length,
    categories: mockUserCategories.length,
    transactions: mockPaymentTransactions.length,
  }
});

export const setupMockAdapter = (apiClient) => {
  apiClient.interceptors.request.use((config) => {
    if (import.meta.env.VITE_USE_MOCK !== 'true') return config;

    const method = (config.method || 'get').toLowerCase();
    const url = config.url || '';
    const params = config.params || {};

    console.log(`[Mock Intercept] ${method.toUpperCase()} ${url}`);

    const error = new Error('mock-intercepted');

    // --- AUTH ENDPOINTS ---
    if (url.includes('/auth/login') || (url.includes('/Authenticator/singin') && !url.includes('/Authenticator/singin-sso'))) {
      const body = parseBody(config.data);
      const user = mockUsers.find(u => u.email === body.email && u.password === body.password);
      if (user) {
        error.mockData = { status: 200, data: mockResponses.loginSuccess(user) };
      } else {
        error.mockData = {
          status: 401,
          data: { success: false, messages: ["Invalid email or password. Check the Mock Guide."] }
        };
      }
    } else if (url.includes('/auth/register') || url.includes('/Users/singup') || url.includes('/User/singup')) {
      const body = parseBody(config.data);
      const exists = mockUsers.find(u => u.email === body.email);
      if (exists) {
        error.mockData = { status: 400, data: { success: false, messages: ["Email already in use."] } };
      } else {
        const newUser = { id: mockUsers.length + 1, name: body.name || body.fullName || 'New User', email: body.email, password: body.password };
        mockUsers.push(newUser);
        error.mockData = { status: 201, data: mockResponses.loginSuccess(newUser) };
      }
    } else if (url.includes('/Authenticator/accept-terms:singup')) {
      error.mockData = {
        status: 200,
        data: {
          success: true,
          data: {
            version: '1.0',
            content: 'Welcome to Nievo EasyFin. By creating an account, you agree to manage your financial data responsibly.',
          }
        }
      };
    } else if (url.includes('/Authenticator/validate:email') || url.includes('/Authenticator/send-validate:email')) {
      error.mockData = { status: 200, data: { success: true, data: 'Email verification processed.' } };
    } else if (url.includes('/auth/change-password') || (url.includes('/password-reset') && method === 'patch')) {
      const body = parseBody(config.data);
      if ((body.pin_token || body.oldPassword) && (body.password || body.newPassword)) {
        const user = mockUsers.find(u => u.email === body.email) || mockUsers[0];
        if (user) {
          user.password = body.password || body.newPassword;
          error.mockData = { status: 200, data: { success: true, message: "Password updated." } };
        } else {
          error.mockData = { status: 404, data: { success: false, messages: ["User not found"] } };
        }
      } else {
        error.mockData = { status: 400, data: { success: false, messages: ["Invalid or missing credentials"] } };
      }
    } else if (url.includes('/auth/forgot-password') || (url.includes('/password-reset') && method === 'post')) {
      error.mockData = { status: 200, data: { success: true, message: "Reset link sent." } };

    // --- DASHBOARD (kept intact per instructions) ---
    } else if (url.includes('/dashboard')) {
      error.mockData = { status: 200, data: mockResponses.dashboard };

    // --- CORE: USER BANKS (/api/public/v1/User/banks) ---
    } else if (url.includes('/User/banks')) {
      if (method === 'get') {
        error.mockData = {
          status: 200,
          data: {
            success: true,
            data: mockUserBanks,
          },
        };
      } else if (method === 'post') {
        const body = parseBody(config.data);
        const catalogBank = mockCatalogBanks.find(
          b => b.name.toLowerCase() === (body.bank_name || '').toLowerCase() && Number(b.bank_type) === Number(body.bank_type)
        ) || mockCatalogBanks.find(b => b.name.toLowerCase() === (body.bank_name || '').toLowerCase());

        if (!catalogBank) {
          error.mockData = { status: 404, data: { success: false, messages: ['Selected bank institution was not found in the catalog.'] } };
        } else {
          const exists = mockUserBanks.find(ub => ub.name.toLowerCase() === catalogBank.name.toLowerCase());
          if (exists) {
            error.mockData = { status: 400, data: { success: false, messages: ['This bank account is already linked to your profile.'] } };
          } else {
            const newEntry = {
              id: mockUserBanks.length + 1,
              bank_id: catalogBank.bank_id,
              name: catalogBank.name,
              nick_name: body.nickname || `${catalogBank.name} Account`,
              bank_type: catalogBank.bank_type_name,
            };
            mockUserBanks = [...mockUserBanks, newEntry];
            error.mockData = { status: 200, data: { success: true, data: 'User bank account linked successfully.' } };
          }
        }
      }

    // --- CORE: USER BANK CARDS (/api/public/v1/User/bank-card) ---
    } else if (url.includes('/User/bank-card')) {
      if (method === 'get') {
        let filtered = [...mockUserBankCards];
        if (params.bank_id) {
          filtered = filtered.filter(c => Number(c.bank_id) === Number(params.bank_id));
        }
        if (params.flag) {
          filtered = filtered.filter(c => c.bank_card_flag.toLowerCase() === String(params.flag).toLowerCase());
        }
        if (params.active !== undefined) {
          const isActive = String(params.active) === 'true';
          filtered = filtered.filter(c => c.active === isActive);
        }
        error.mockData = { status: 200, data: wrapPaginated(filtered, params.page, params.page_size) };
      } else if (method === 'post') {
        const body = parseBody(config.data);
        const bank = mockCatalogBanks.find(b => Number(b.bank_id) === Number(body.bank_id));
        const catalogCard = mockCatalogBankCards.find(c => Number(c.id) === Number(body.card_id));
        if (!bank || !catalogCard) {
          error.mockData = { status: 404, data: { success: false, messages: ['Bank or card catalog product not found.'] } };
        } else {
          const newCard = {
            user_bank_card_id: mockUserBankCards.length + 1,
            user_bank_card_nickname: body.nickname || catalogCard.card_name,
            active: true,
            expired_at: body.expire_at || '2029-12-31T00:00:00Z',
            bank_name: bank.name,
            bank_id: bank.bank_id,
            bank_card_name: catalogCard.card_name,
            bank_card_type: catalogCard.card_type,
            bank_card_flag: catalogCard.card_flag,
          };
          mockUserBankCards = [...mockUserBankCards, newCard];
          error.mockData = { status: 200, data: { success: true, data: 'User card registered successfully.' } };
        }
      }

    // --- CORE: USER GOALS (/api/public/v1/User/goal) ---
    } else if (url.includes('/User/goal')) {
      if (method === 'get') {
        let filtered = [...mockUserGoals];
        if (params.active !== undefined) {
          const isActive = String(params.active) === 'true';
          filtered = filtered.filter(g => g.active === isActive);
        }
        error.mockData = { status: 200, data: wrapPaginated(filtered, params.page, params.page_size) };
      } else if (method === 'post') {
        const body = parseBody(config.data);
        if (!body.name || Number(body.amount) <= 0) {
          error.mockData = { status: 400, data: { success: false, messages: ['Please provide a valid goal name and positive amount.'] } };
        } else if (mockUserGoals.some(g => g.name.toLowerCase() === body.name.toLowerCase() && g.active)) {
          error.mockData = { status: 400, data: { success: false, messages: ['An active goal with this name already exists.'] } };
        } else {
          const newGoal = {
            id: mockUserGoals.length + 1,
            name: body.name,
            description: body.description || '',
            amount: Number(body.amount),
            is_percent: Boolean(body.is_percent),
            active: true,
            expire_at: body.expire_at || '2027-12-31T00:00:00Z',
            created_at: new Date().toISOString(),
            updated_at: new Date().toISOString(),
          };
          mockUserGoals = [newGoal, ...mockUserGoals];
          error.mockData = { status: 200, data: { success: true, data: 'Goal created successfully.' } };
        }
      }

    // --- CORE: USER CATEGORIES (/api/public/v1/User/category) ---
    } else if (url.includes('/User/category')) {
      if (method === 'get') {
        let filtered = [...mockUserCategories];
        if (params.active !== undefined) {
          const isActive = String(params.active) === 'true';
          filtered = filtered.filter(c => c.active === isActive);
        }
        error.mockData = { status: 200, data: wrapPaginated(filtered, params.page, params.page_size) };
      } else if (method === 'post') {
        const body = parseBody(config.data);
        const hasGoal = body.goal !== null && body.goal !== undefined && Number(body.goal) > 0;
        const hasParent = body.parent_category !== null && body.parent_category !== undefined && Number(body.parent_category) > 0;

        if ((hasGoal && hasParent) || (!hasGoal && !hasParent)) {
          error.mockData = {
            status: 400,
            data: {
              success: false,
              messages: ['A category must be linked exclusively to either a Goal OR a Parent Category.']
            }
          };
        } else if (mockUserCategories.some(c => c.name.toLowerCase() === (body.name || '').toLowerCase() && c.active)) {
          error.mockData = { status: 400, data: { success: false, messages: ['Category with this name already exists.'] } };
        } else {
          const linkedGoal = hasGoal ? mockUserGoals.find(g => Number(g.id) === Number(body.goal)) : null;
          const newCat = {
            id: String(mockUserCategories.length + 1),
            name: body.name,
            description: body.description || '',
            active: true,
            parent_category: hasParent ? Number(body.parent_category) : null,
            goal_name: linkedGoal ? linkedGoal.name : '',
            goal_description: linkedGoal ? linkedGoal.description : '',
            created_at: new Date().toISOString(),
            updated_at: new Date().toISOString(),
          };
          mockUserCategories = [newCat, ...mockUserCategories];
          error.mockData = { status: 200, data: { success: true, data: 'Category created successfully.' } };
        }
      }

    // --- CORE: BANK CATALOG (/api/public/v1/Bank/card & /api/public/v1/Bank) ---
    } else if (url.includes('/Bank/card')) {
      let filtered = [...mockCatalogBankCards];
      if (params.bank) {
        filtered = filtered.filter(c => Number(c.bank_id) === Number(params.bank));
      }
      if (params.card_type) {
        filtered = filtered.filter(c => Number(c.card_type_id) === Number(params.card_type));
      }
      if (params.flag) {
        filtered = filtered.filter(c => c.card_flag.toLowerCase() === String(params.flag).toLowerCase());
      }
      error.mockData = { status: 200, data: wrapPaginated(filtered, params.page, params.page_size) };
    } else if (url.includes('/Bank')) {
      error.mockData = { status: 200, data: wrapPaginated(mockCatalogBanks, params.page, params.page_size) };

    // --- CORE: CARD METADATA (/api/public/v1/Card/type & /api/public/v1/Card/flag) ---
    } else if (url.includes('/Card/type')) {
      error.mockData = { status: 200, data: wrapPaginated(mockCardTypes, params.page, params.page_size) };
    } else if (url.includes('/Card/flag')) {
      error.mockData = { status: 200, data: wrapPaginated(mockCardFlags, params.page, params.page_size) };

    // --- CORE: PAYMENT & TRANSACTIONS (/api/public/v1/Payment...) ---
    } else if (url.includes('/Payment/status')) {
      error.mockData = { status: 200, data: wrapPaginated(mockTransactionStatuses, params.page, params.page_size) };
    } else if (url.includes('/Payment/type')) {
      error.mockData = { status: 200, data: wrapPaginated(mockTransactionTypes, params.page, params.page_size) };
    } else if (url.includes('/Payment/scheduler')) {
      error.mockData = { status: 200, data: wrapPaginated(mockSchedulers, params.page, params.page_size) };
    } else if (url.includes('/Payment')) {
      if (method === 'get') {
        let filtered = [...mockPaymentTransactions];
        if (params.transaction_status_id) {
          const st = mockTransactionStatuses.find(s => Number(s.id) === Number(params.transaction_status_id));
          if (st) filtered = filtered.filter(t => t.status === st.name);
        }
        if (params.transaction_type_id) {
          const tp = mockTransactionTypes.find(t => Number(t.id) === Number(params.transaction_type_id));
          if (tp) filtered = filtered.filter(t => t.type === tp.name);
        }
        if (params.category) {
          const cat = mockUserCategories.find(c => String(c.id) === String(params.category));
          if (cat) filtered = filtered.filter(t => t.category === cat.name);
        }
        if (params.user_bank_card) {
          const uCard = mockUserBankCards.find(c => Number(c.user_bank_card_id) === Number(params.user_bank_card));
          if (uCard) filtered = filtered.filter(t => t.user_card_name === uCard.user_bank_card_nickname);
        }
        if (params.scheduler_exists !== undefined && params.scheduler_exists !== '') {
          const hasSched = String(params.scheduler_exists) === 'true';
          filtered = filtered.filter(t => hasSched ? Boolean(t.cron_tab) : !t.cron_tab);
        }
        error.mockData = { status: 200, data: wrapPaginated(filtered, params.page, params.page_size) };
      } else if (method === 'post') {
        const body = parseBody(config.data);
        const st = mockTransactionStatuses.find(s => Number(s.id) === Number(body.transaction_status)) || mockTransactionStatuses[0];
        const tp = mockTransactionTypes.find(t => Number(t.id) === Number(body.transaction_type)) || mockTransactionTypes[0];
        const sched = Number(body.scheduler) > 0 ? mockSchedulers.find(s => Number(s.id) === Number(body.scheduler)) : null;
        const userBank = mockUserBanks.find(b => Number(b.bank_id) === Number(body.bank) || Number(b.id) === Number(body.bank)) || mockUserBanks[0];
        const userCard = Number(body.card) > 0
          ? mockUserBankCards.find(c => Number(c.user_bank_card_id) === Number(body.card))
          : null;

        const newTx = {
          id: `mock-tx-${Date.now()}`,
          name: body.name,
          description: body.description,
          amount: Number(body.amount),
          cron_tab: sched?.cron_tab || '',
          status: st.name,
          type: tp.name,
          category: body.category_name,
          pay_date: body.pay_date || new Date().toISOString(),
          user_card_name: userCard ? userCard.user_bank_card_nickname : '',
          user_bank_nickname: userBank ? userBank.nick_name : 'Main Account',
          bank_name: userBank ? userBank.name : 'Itaú Unibanco',
          created_at: new Date().toISOString(),
        };
        mockPaymentTransactions = [newTx, ...mockPaymentTransactions];
        error.mockData = { status: 201, data: { success: true, data: 'Payment transaction created successfully.' } };
      }
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
