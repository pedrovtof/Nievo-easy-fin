import { styled } from '@mui/material/styles';
import { Box, Card, LinearProgress, linearProgressClasses } from '@mui/material';

export const BudgetContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4),
}));

export const SummaryGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))',
  gap: theme.spacing(3),
}));

export const SummaryCard = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2),
  backgroundColor: theme.palette.surface.high,
}));

export const CategoryGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))',
  gap: theme.spacing(3),
}));

export const CategoryCard = styled(Card, {
  shouldForwardProp: (prop) => prop !== 'status',
})(({ theme, status }) => ({
  padding: theme.spacing(3),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2),
  border: status === 'over' ? `1px solid ${theme.palette.error.main}` : 'none',
  backgroundColor: theme.palette.background.paper,
  position: 'relative',
}));

export const StyledLinearProgress = styled(LinearProgress, {
  shouldForwardProp: (prop) => prop !== 'status',
})(({ theme, status }) => ({
  height: 12,
  borderRadius: 6,
  [`&.${linearProgressClasses.colorPrimary}`]: {
    backgroundColor: theme.palette.action.hover,
  },
  [`& .${linearProgressClasses.bar}`]: {
    borderRadius: 6,
    backgroundColor: status === 'over' ? theme.palette.error.main : theme.palette.primary.main,
  },
}));
