import { styled } from '@mui/material/styles';
import { Box } from '@mui/material';

export const LayoutContainer = styled(Box)(({ theme }) => ({
  backgroundColor: theme.palette.background.default,
  fontFamily: theme.typography.fontFamily,
  display: 'flex',
  height: '100vh',
  width: '100vw',
  overflow: 'hidden',
  color: theme.palette.text.primary,
}));

export const MainContent = styled(Box)({
  flex: 1,
  display: 'flex',
  flexDirection: 'column',
  height: '100%',
  overflow: 'hidden',
  position: 'relative',
});

export const ContentArea = styled(Box)(({ theme }) => ({
  flex: 1,
  overflowY: 'auto',
  padding: theme.spacing(4),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4),
}));
