import { styled } from '@mui/material/styles';
import { Box, Card } from '@mui/material';

export const CardsWrapper = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4.5),
}));

export const FilterStrip = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  flexWrap: 'wrap',
  gap: theme.spacing(2),
  padding: theme.spacing(2.5, 3),
  backgroundColor: theme.palette.surface.main,
  borderRadius: theme.shape.borderRadius,
}));

export const WalletGrid = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fill, minmax(320px, 1fr))',
  gap: theme.spacing(3),
}));

export const CreditCardVisual = styled(Card, {
  shouldForwardProp: (prop) => prop !== 'primaryCard',
})(({ theme, primaryCard }) => ({
  padding: theme.spacing(3.5),
  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'space-between',
  minHeight: 200,
  background: primaryCard
    ? `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dim || '#3e4652'} 100%)`
    : theme.palette.surface.high,
  color: primaryCard ? '#ffffff' : theme.palette.text.primary,
  border: 'none',
  position: 'relative',
  overflow: 'hidden',
}));

export const CatalogRow = styled(Box)(({ theme }) => ({
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  flexWrap: 'wrap',
  gap: theme.spacing(2),
  padding: theme.spacing(2.5, 3),
  backgroundColor: theme.palette.background.paper,
  borderRadius: theme.shape.borderRadius,
}));
