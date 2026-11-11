import { styled } from '@mui/material/styles';
import { Box, Card, LinearProgress, linearProgressClasses } from '@mui/material';

export const BudgetContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4.5),
}));

export const SummaryGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
  gap: theme.spacing(3),
}));

export const SummaryCard = styled(Card, {
  shouldForwardProp: (prop) => prop !== 'featured',
})(({ theme, featured }) => ({
  padding: theme.spacing(3.5),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(1.5),
  background: featured
    ? `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dim || '#3e4652'} 100%)`
    : theme.palette.surface.high,
  color: featured ? '#ffffff' : theme.palette.text.primary,
  border: 'none',
}));

export const CategoryGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fill, minmax(320px, 1fr))',
  gap: theme.spacing(3),
}));

export const CategoryCard = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2.5),
  border: 'none',
  backgroundColor: theme.palette.background.paper,
  position: 'relative',
}));

export const StyledLinearProgress = styled(LinearProgress, {
  shouldForwardProp: (prop) => prop !== 'isPercent',
})(({ theme, isPercent }) => ({
  height: 10,
  borderRadius: 6,
  [`&.${linearProgressClasses.colorPrimary}`]: {
    backgroundColor: theme.palette.surface.highest,
  },
  [`& .${linearProgressClasses.bar}`]: {
    borderRadius: 6,
    backgroundColor: isPercent ? theme.palette.tertiary.main : theme.palette.primary.main,
  },
}));
