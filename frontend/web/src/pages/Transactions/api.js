import api from '../../services/api';

export const getTransactionsData = () => {
  return Promise.resolve({
    data: {
      transactions: [
        {
          id: 1, date: 'Jun 24, 2023', time: '10:42 AM', icon: 'sports_esports', title: 'Steam Purchase',
          account: 'Debit Card ••4291', category: 'Games', amount: -59.99, type: 'expense'
        },
        {
          id: 2, date: 'Jun 23, 2023', time: '04:15 PM', icon: 'shopping_cart', title: 'Local Market Grocery',
          account: 'Cash Wallet', category: 'Market', amount: -120.50, type: 'expense'
        },
        {
          id: 3, date: 'Jun 22, 2023', time: '09:00 AM', icon: 'payments', title: 'Freelance Payment',
          account: 'Bank Transfer', category: 'Income', amount: 450.00, type: 'income'
        },
        {
          id: 4, date: 'Jun 21, 2023', time: '06:30 PM', icon: 'local_gas_station', title: 'Shell Gas Station',
          account: 'Credit Card ••8821', category: 'Transport', amount: -45.00, type: 'expense'
        }
      ]
    }
  });
};
