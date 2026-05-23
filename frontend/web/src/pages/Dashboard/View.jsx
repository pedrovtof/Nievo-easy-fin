import React from 'react';
import { Typography, Box, CircularProgress, Chip } from '@mui/material';
import { useText } from '../../hooks/useText';
import { DashboardContainer, SummaryGrid, SummaryCard, TransactionList, TransactionItem } from './styles';

/**
 * Dashboard View Template
 * Pure presentation component to display the financial overview metrics.
 *
 * @param {Object} props - Contains dashboard data, loading state, and error message.
 * @returns {JSX.Element} The rendered visual layer of the Dashboard page.
 */
const DashboardView = ({ data, loading, error }) => {
  const { t } = useText();

  if (loading) {
    return <Box display="flex" justifyContent="center" p={4}><CircularProgress /></Box>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  if (!data) return null;

  return (
    <DashboardContainer>
      <SummaryGrid>
        <SummaryCard variant="primary">
          <Typography variant="body2" color="secondary">{t('Dashboard.totalBalance')}</Typography>
          <Typography variant="h2" color="primary">${data.totalBalance.toFixed(2)}</Typography>
        </SummaryCard>
        <SummaryCard>
          <Typography variant="body2" color="secondary">{t('Dashboard.income')}</Typography>
          <Typography variant="h3" color="success.main">+${data.income.toFixed(2)}</Typography>
        </SummaryCard>
        <SummaryCard>
          <Typography variant="body2" color="secondary">{t('Dashboard.expenses')}</Typography>
          <Typography variant="h3" color="error.main">-${data.expenses.toFixed(2)}</Typography>
        </SummaryCard>
      </SummaryGrid>

      <Box>
        <Typography variant="h3" mb={3}>{t('Dashboard.recentTransactions')}</Typography>
        <TransactionList>
          {data.recentTransactions?.map(tx => (
            <TransactionItem key={tx.id}>
              <Box display="flex" alignItems="center" gap={2}>
                <Box>
                  <Typography variant="body1" fontWeight={600}>{tx.description}</Typography>
                  <Typography variant="caption" color="secondary">{tx.date}</Typography>
                </Box>
              </Box>
              <Box display="flex" alignItems="center" gap={2}>
                <Chip label={tx.category} size="small" variant="outlined" />
                <Typography variant="body1" fontWeight={600} color={tx.amount > 0 ? 'success.main' : 'error.main'}>
                  {tx.amount > 0 ? '+' : ''}{tx.amount.toFixed(2)}
                </Typography>
              </Box>
            </TransactionItem>
          ))}
        </TransactionList>
      </Box>
    </DashboardContainer>
  );
};

export default DashboardView;
