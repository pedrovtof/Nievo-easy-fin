/**
 * Budget API definitions
 * Export functions here that interact with the /budget endpoints.
 */

export const getBudgetData = () => {
  // For now we mock it directly if there's no endpoint.
  return Promise.resolve({
    data: {
      totalBudget: 2400,
      leftToSpend: 850,
      categories: [
        {
          title: 'Grocery',
          type: 'Essential',
          icon: 'shopping_cart',
          amountSpent: 350,
          limit: 400,
          percentage: 87.5,
          status: 'normal',
        },
        {
          title: 'Leisure',
          type: 'Discretionary',
          icon: 'theaters',
          amountSpent: 120,
          limit: 100,
          percentage: 120,
          status: 'over',
          warning: 'Exceeded by $20.00'
        }
      ]
    }
  });
};
