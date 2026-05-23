import React from 'react';
import { Typography, Box, CircularProgress, Button, Table, TableRow, TableBody, Chip, Icon } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { useText } from '../../hooks/useText';
import { TransactionsWrapper, FiltersBar, StyledTableContainer, StyledTableHead, HeaderCell, BodyCell } from './styles';

const TransactionsView = ({ data, loading, error }) => {
  const { t } = useText();

  if (loading) {
    return <Box display="flex" justifyContent="center" p={4}><CircularProgress /></Box>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  if (!data) return null;

  return (
    <TransactionsWrapper>
      <Box display="flex" justifyContent="space-between" alignItems="center" flexWrap="wrap" gap={2}>
        <Box>
          <Typography variant="h2" color="primary">{t('Transactions.title')}</Typography>
          <Typography variant="body2" color="text.secondary">Track your spending and income across all accounts.</Typography>
        </Box>
        <Button variant="contained" color="secondary" startIcon={<AddIcon />}>
          {t('Transactions.addTransaction')}
        </Button>
      </Box>

      <FiltersBar>
        <Typography variant="body2" fontWeight="bold" color="text.secondary" textTransform="uppercase">Filters:</Typography>
        <Chip label="Period: This Month" onClick={() => {}} />
        <Chip label="Account: All Accounts" onClick={() => {}} />
        <Chip label="Category: Entertainment" onClick={() => {}} />
      </FiltersBar>

      <StyledTableContainer>
        <Table>
          <StyledTableHead>
            <TableRow>
              <HeaderCell>{t('Transactions.date')}</HeaderCell>
              <HeaderCell>{t('Transactions.description')}</HeaderCell>
              <HeaderCell>Account</HeaderCell>
              <HeaderCell>{t('Transactions.category')}</HeaderCell>
              <HeaderCell align="right">{t('Transactions.amount')}</HeaderCell>
            </TableRow>
          </StyledTableHead>
          <TableBody>
            {data.transactions.map((tx) => (
              <TableRow key={tx.id} hover>
                <BodyCell>
                  <Typography variant="body2" fontWeight="bold">{tx.date}</Typography>
                  <Typography variant="caption" color="text.secondary">{tx.time}</Typography>
                </BodyCell>
                <BodyCell>
                  <Box display="flex" alignItems="center" gap={2}>
                    <Box sx={{ width: 32, height: 32, bgcolor: 'surface.main', display: 'flex', alignItems: 'center', justifyContent: 'center', borderRadius: '50%' }}>
                      <Icon fontSize="small">{tx.icon}</Icon>
                    </Box>
                    <Typography variant="body2" fontWeight="bold">{tx.title}</Typography>
                  </Box>
                </BodyCell>
                <BodyCell>
                  <Typography variant="body2" color="text.secondary">{tx.account}</Typography>
                </BodyCell>
                <BodyCell>
                  <Chip label={tx.category} size="small" variant="outlined" />
                </BodyCell>
                <BodyCell align="right">
                  <Typography variant="body2" fontWeight="bold" color={tx.type === 'income' ? 'success.main' : 'error.main'}>
                    {tx.type === 'expense' ? '-' : '+'}${Math.abs(tx.amount).toFixed(2)}
                  </Typography>
                </BodyCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </StyledTableContainer>
    </TransactionsWrapper>
  );
};

export default TransactionsView;
