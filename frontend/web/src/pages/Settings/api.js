import api from '../../services/api';

export const getSettingsData = () => {
  return Promise.resolve({
    data: {
      profile: {
        fullName: 'Alex Morgan',
        email: 'alex.morgan@example.com',
        currency: 'USD'
      },
      toggles: {
        twoFactor: false,
        darkMode: false,
        weeklyDigest: true,
        unusualActivity: true,
        budgetWarnings: false
      }
    }
  });
};

export const updateSettings = (data) => {
  return Promise.resolve({ success: true, data });
};

export const changePassword = (email, oldPassword, newPassword) => {
  return api.post('/auth/change-password', { email, oldPassword, newPassword });
};
