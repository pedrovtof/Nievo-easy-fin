import { styled } from '@mui/material/styles';
import { Box } from '@mui/material';

export const FabContainer = styled(Box)(({ theme }) => ({
  position: 'fixed',
  bottom: 24,
  right: 24,
  zIndex: 1000,
  '& .MuiFab-root': {
    backgroundColor: theme.palette.tertiary.main,
    '&:hover': {
      backgroundColor: '#004a77',
    },
  },
}));

export const PreformattedText = styled('pre')(({ theme }) => ({
  backgroundColor: theme.palette.background.default,
  padding: theme.spacing(2),
  borderRadius: theme.shape.borderRadius,
  overflowX: 'auto',
  fontSize: '0.85rem',
  fontFamily: 'monospace',
}));
