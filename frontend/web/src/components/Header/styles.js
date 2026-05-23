import { styled } from '@mui/material/styles';
import { AppBar, Box, Typography, IconButton, Button } from '@mui/material';

export const HeaderContainer = styled(AppBar)(({ theme }) => ({
  backgroundColor: 'rgba(255, 255, 255, 0.8)',
  backdropFilter: 'blur(20px)',
  boxShadow: 'none',
  borderBottom: 'none',
  padding: theme.spacing(3, 4),
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-between',
  alignItems: 'center',
  position: 'sticky',
  top: 0,
  zIndex: 10,
  color: theme.palette.text.primary,
}));

export const TitleBox = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(0.5),
}));

export const ActionBox = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  gap: theme.spacing(2),
}));

export const NotificationBadge = styled('span')(({ theme }) => ({
  position: 'absolute',
  top: 8,
  right: 8,
  width: 8,
  height: 8,
  backgroundColor: theme.palette.error.main,
  borderRadius: '50%',
}));
