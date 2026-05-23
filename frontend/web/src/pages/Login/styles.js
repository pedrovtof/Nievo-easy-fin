import { styled } from '@mui/material/styles';
import { Box, Card, Typography, TextField, Button, Alert } from '@mui/material';

export const LoginContainer = styled(Box)(({ theme }) => ({
  backgroundColor: theme.palette.background.default,
  minHeight: '100vh',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  padding: theme.spacing(2),
}));

export const LoginCard = styled(Card)(({ theme }) => ({
  width: '100%',
  maxWidth: 440,
  padding: theme.spacing(4),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(3),
  // Card has shadow set in theme.js
}));

export const LogoHeader = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  marginBottom: theme.spacing(2),
}));

export const StyledForm = styled('form')(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2.5),
}));

export const DividerBox = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  padding: theme.spacing(1, 0),
  '&::before, &::after': {
    content: '""',
    flex: 1,
    borderBottom: `1px solid ${theme.palette.divider}`,
  },
}));

export const DividerText = styled(Typography)(({ theme }) => ({
  padding: theme.spacing(0, 2),
  color: theme.palette.text.secondary,
  textTransform: 'uppercase',
  fontSize: '0.75rem',
  fontWeight: 500,
  letterSpacing: '0.05em',
}));

export const SsoButton = styled(Button)(({ theme }) => ({
  height: 48,
  backgroundColor: theme.palette.background.paper,
  color: theme.palette.text.primary,
  border: `1px solid ${theme.palette.divider}`,
  '&:hover': {
    backgroundColor: theme.palette.action.hover,
  },
}));
