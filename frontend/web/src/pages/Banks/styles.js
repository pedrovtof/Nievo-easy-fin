import { styled } from '@mui/material/styles';
import { Box, Card } from '@mui/material';

export const BanksWrapper = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4.5),
}));

export const HeroGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fit, minmax(260px, 1fr))',
  gap: theme.spacing(3),
}));

export const HeroCard = styled(Card, {
  shouldForwardProp: (prop) => prop !== 'featured',
})(({ theme, featured }) => ({
  padding: theme.spacing(3.5),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(1),
  background: featured
    ? `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dim || '#3e4652'} 100%)`
    : theme.palette.surface.high,
  color: featured ? '#ffffff' : theme.palette.text.primary,
  border: 'none',
}));

export const AccountsGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
  gap: theme.spacing(3),
}));

export const AccountCard = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'space-between',
  gap: theme.spacing(2.5),
  backgroundColor: theme.palette.background.paper,
  border: 'none',
  transition: 'transform 0.2s ease',
  '&:hover': {
    transform: 'translateY(-2px)',
  },
}));

export const CatalogCard = styled(Box)(({ theme }) => ({
  padding: theme.spacing(2.5, 3),
  borderRadius: theme.shape.borderRadius,
  backgroundColor: theme.palette.surface.main,
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  gap: theme.spacing(2),
}));

export const IconBadge = styled(Box)(({ theme }) => ({
  width: 48,
  height: 48,
  borderRadius: 16,
  backgroundColor: theme.palette.surface.high,
  color: theme.palette.primary.main,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
}));
