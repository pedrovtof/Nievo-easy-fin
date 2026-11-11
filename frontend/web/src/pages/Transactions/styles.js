import { styled } from '@mui/material/styles';
import { Box } from '@mui/material';

export const TransactionsWrapper = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4),
}));

export const FiltersBar = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  flexWrap: 'wrap',
  gap: theme.spacing(2),
  padding: theme.spacing(2.5, 3),
  backgroundColor: theme.palette.surface.main,
  borderRadius: theme.shape.borderRadius,
  border: 'none',
}));

export const TransactionsListContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2),
}));

export const TransactionRow = styled(Box, {
  shouldForwardProp: (prop) => prop !== 'alternate',
})(({ theme, alternate }) => ({
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  flexWrap: 'wrap',
  gap: theme.spacing(2),
  padding: theme.spacing(2.5, 3),
  backgroundColor: alternate ? theme.palette.surface.main : theme.palette.background.paper,
  borderRadius: theme.shape.borderRadius,
  border: 'none',
  transition: 'transform 0.15s ease',
  '&:hover': {
    transform: 'translateX(4px)',
  },
}));
