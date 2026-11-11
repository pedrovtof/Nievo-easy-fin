import React from 'react';
import { Typography, Box } from '@mui/material';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import DashboardIcon from '@mui/icons-material/Dashboard';
import ReceiptLongIcon from '@mui/icons-material/ReceiptLong';
import AccountBalanceIcon from '@mui/icons-material/AccountBalance';
import CreditCardIcon from '@mui/icons-material/CreditCard';
import DonutLargeIcon from '@mui/icons-material/DonutLarge';
import CategoryIcon from '@mui/icons-material/Category';
import SettingsIcon from '@mui/icons-material/Settings';
import { SidebarContainer, LogoBox, LogoIconWrapper, NavContainer, NavItemLink } from './styles';
import { useText } from '../../hooks/useText';

const ICONS = {
  dashboard: DashboardIcon,
  receipt_long: ReceiptLongIcon,
  account_balance: AccountBalanceIcon,
  credit_card: CreditCardIcon,
  donut_large: DonutLargeIcon,
  category: CategoryIcon,
  settings: SettingsIcon,
};

/**
 * Sidebar Navigation View
 * Pure presentation layer rendering navigation items with icons and labels.
 *
 * @param {Object} props - Contains navItems array and currentPath string.
 * @returns {JSX.Element} The rendered Sidebar visual tree.
 */
const SidebarView = ({ navItems, currentPath }) => {
  const { t } = useText();

  return (
    <SidebarContainer>
      <LogoBox>
        <LogoIconWrapper>
          <AccountBalanceWalletIcon />
        </LogoIconWrapper>
        <Box display="flex" flexDirection="column">
          <Typography variant="subtitle1" color="primary" sx={{ fontWeight: 'bold', lineHeight: 1 }}>
            FinControl
          </Typography>
          <Typography variant="caption" color="secondary">
            Personal Finance
          </Typography>
        </Box>
      </LogoBox>

      <NavContainer>
        {navItems.map((item) => {
          const active = currentPath === item.path;
          const Icon = ICONS[item.icon] || DashboardIcon;
          return (
            <NavItemLink key={item.label} to={item.path} active={active ? 1 : 0}>
              <Icon />
              <Typography variant="body2" sx={{ fontWeight: 'inherit' }}>
                {t(`Navigation.${item.label}`)}
              </Typography>
            </NavItemLink>
          );
        })}
      </NavContainer>
    </SidebarContainer>
  );
};

export default SidebarView;
