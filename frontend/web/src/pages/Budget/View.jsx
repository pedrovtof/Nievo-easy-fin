import React from 'react';
import { Typography, Box, CircularProgress, Icon } from '@mui/material';
import { useText } from '../../hooks/useText';
import { BudgetContainer, SummaryGrid, SummaryCard, CategoryGrid, CategoryCard, StyledLinearProgress } from './styles';

const BudgetView = ({ data, loading, error }) => {
  const { t } = useText();

  if (loading) {
    return <Box display="flex" justifyContent="center" p={4}><CircularProgress /></Box>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  if (!data) return null;

  return (
    <BudgetContainer>
      <SummaryGrid>
        <SummaryCard>
          <Typography variant="body2" color="secondary" textTransform="uppercase" fontWeight="600">Total Monthly Budget</Typography>
          <Typography variant="h2" color="primary">${data.totalBudget.toFixed(2)}</Typography>
        </SummaryCard>
        <SummaryCard>
          <Typography variant="body2" color="secondary" textTransform="uppercase" fontWeight="600">Left to Spend</Typography>
          <Typography variant="h2" color="primary">${data.leftToSpend.toFixed(2)}</Typography>
        </SummaryCard>
      </SummaryGrid>

      <Box>
        <Typography variant="h3" mb={3}>Category Budgets</Typography>
        <CategoryGrid>
          {data.categories.map((cat, idx) => (
            <CategoryCard key={idx} status={cat.status}>
              <Box display="flex" justifyContent="space-between" alignItems="flex-start">
                <Box display="flex" gap={2} alignItems="center">
                  <Box sx={{ p: 1.5, borderRadius: 2, bgcolor: 'surface.main', display: 'flex' }}>
                    <Icon color="primary">{cat.icon}</Icon>
                  </Box>
                  <Box>
                    <Typography variant="h6" fontWeight="bold" color="primary">{cat.title}</Typography>
                    <Typography variant="caption" color="secondary">{cat.type}</Typography>
                  </Box>
                </Box>
              </Box>

              <Box mt="auto">
                <Box display="flex" justifyContent="space-between" alignItems="flex-end" mb={1}>
                  <Box>
                    <Typography variant="h4" color={cat.status === 'over' ? 'error.main' : 'primary'} fontWeight="bold">
                      ${cat.amountSpent}
                    </Typography>
                    <Typography variant="caption" color={cat.status === 'over' ? 'error.main' : 'text.secondary'}>
                      spent of ${cat.limit}
                    </Typography>
                  </Box>
                  <Typography variant="h6" color={cat.status === 'over' ? 'error.main' : 'primary'} fontWeight="bold">
                    {cat.percentage}%
                  </Typography>
                </Box>
                <StyledLinearProgress variant="determinate" value={Math.min(cat.percentage, 100)} status={cat.status} />
                
                <Box display="flex" justifyContent="space-between" mt={1}>
                  {cat.status === 'over' ? (
                    <Typography variant="caption" color="error.main" fontWeight="bold">{cat.warning}</Typography>
                  ) : (
                    <Typography variant="caption" color="text.secondary">Left: ${(cat.limit - cat.amountSpent).toFixed(2)}</Typography>
                  )}
                </Box>
              </Box>
            </CategoryCard>
          ))}
        </CategoryGrid>
      </Box>
    </BudgetContainer>
  );
};

export default BudgetView;
