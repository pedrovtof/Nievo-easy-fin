import { styled } from '@mui/material/styles';
import { Box, Card } from '@mui/material';

export const CategoriesWrapper = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4.5),
}));

export const StatsGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fit, minmax(240px, 1fr))',
  gap: theme.spacing(3),
}));

export const StatCard = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(1),
  backgroundColor: theme.palette.surface.high,
  border: 'none',
}));

export const CategoryList = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2),
}));

export const CategoryRow = styled(Box, {
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
}));
