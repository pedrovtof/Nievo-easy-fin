import { styled } from '@mui/material/styles';
import { Box, Typography } from '@mui/material';
import { Link } from 'react-router-dom';

export const SidebarContainer = styled(Box)(({ theme }) => ({
  width: 256,
  flexShrink: 0,
  display: 'flex',
  flexDirection: 'column',
  backgroundColor: theme.palette.background.paper,
  height: '100%',
  overflowY: 'auto',
  boxShadow: 'none',
  borderRight: `none`, // The Silent Navigator - no line rule
}));

export const LogoBox = styled(Box)(({ theme }) => ({
  padding: theme.spacing(3),
  display: 'flex',
  alignItems: 'center',
  gap: theme.spacing(1.5),
}));

export const LogoIconWrapper = styled(Box)(({ theme }) => ({
  backgroundColor: theme.palette.primary.main,
  color: theme.palette.primary.contrastText,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  borderRadius: theme.shape.borderRadius / 2,
  width: 40,
  height: 40,
}));

export const NavContainer = styled('nav')(({ theme }) => ({
  flex: 1,
  padding: theme.spacing(1, 2),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(0.5),
}));

export const NavItemLink = styled(Link, {
  shouldForwardProp: (prop) => prop !== 'active',
})(({ theme, active }) => ({
  display: 'flex',
  alignItems: 'center',
  gap: theme.spacing(1.5),
  padding: theme.spacing(1.5),
  borderRadius: theme.shape.borderRadius,
  textDecoration: 'none',
  color: active ? theme.palette.primary.main : theme.palette.secondary.main,
  backgroundColor: active ? 'rgba(85, 95, 113, 0.1)' : 'transparent',
  fontWeight: active ? 600 : 500,
  transition: 'background-color 0.2s',
  '&:hover': {
    backgroundColor: active ? 'rgba(85, 95, 113, 0.15)' : 'rgba(90, 96, 100, 0.05)',
  },
}));
