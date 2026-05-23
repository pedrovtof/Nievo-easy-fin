import { styled } from '@mui/material/styles';
import { Box, Card, Typography, Button } from '@mui/material';

export const RegisterContainer = styled(Box)(({ theme }) => ({
  backgroundColor: theme.palette.background.default,
  minHeight: '100vh',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  padding: theme.spacing(2),
}));

export const RegisterCard = styled(Card)(({ theme }) => ({
  width: '100%',
  maxWidth: 480,
  padding: theme.spacing(4),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(3),
  // Card has shadow set in theme.js
}));

export const HeaderBox = styled(Box)(({ theme }) => ({
  textAlign: 'center',
  marginBottom: theme.spacing(2),
}));

export const StyledForm = styled('form')(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2.5),
}));

export const SsoButton = styled(Button)(({ theme }) => ({
  height: 56,
  backgroundColor: theme.palette.surface.high,
  color: theme.palette.text.primary,
  borderRadius: theme.shape.borderRadius,
  border: 'none',
  fontWeight: 600,
  '&:hover': {
    backgroundColor: theme.palette.surface.highest,
  },
}));
