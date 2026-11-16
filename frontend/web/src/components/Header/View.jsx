import React from 'react';
import { Typography, IconButton, Button, Box, Chip } from '@mui/material';
import NotificationsIcon from '@mui/icons-material/Notifications';
import AddIcon from '@mui/icons-material/Add';
import LogoutIcon from '@mui/icons-material/Logout';
import { HeaderContainer, TitleBox, ActionBox, NotificationBadge } from './styles';
import { useText } from '../../hooks/useText';

/**
 * Header View Template
 * Pure presentation layer rendering personalized greeting, account status badge, and quick actions.
 *
 * @param {Object} props - Properties passed from the Header controller.
 * @returns {JSX.Element} The rendered Header visual layer.
 */
const HeaderView = ({ userName, userStatus, dateString, onNewTransaction, onNotifications, onLogout }) => {
  const { t } = useText();

  return (
    <HeaderContainer position="static">
      <TitleBox>
        <Box display="flex" alignItems="center" gap={1.5} flexWrap="wrap">
          <Typography variant="h3" color="primary">
            Good morning, {userName}
          </Typography>
          {userStatus && (
            <Chip
              label={userStatus}
              size="small"
              sx={{
                bgcolor: 'surface.high',
                color: 'tertiary.main',
                fontWeight: 600,
                fontSize: '0.7rem',
                letterSpacing: 0.5,
                textTransform: 'uppercase',
              }}
            />
          )}
        </Box>
        <Typography variant="subtitle1" color="secondary">
          Here&apos;s your financial overview for {dateString}
        </Typography>
      </TitleBox>
      <ActionBox>
        <IconButton onClick={onNotifications} style={{ position: 'relative' }}>
          <NotificationsIcon color="secondary" />
          <NotificationBadge />
        </IconButton>
        <IconButton onClick={onLogout} color="error" title="Logout">
          <LogoutIcon />
        </IconButton>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={onNewTransaction}
        >
          {t('Transactions.addTransaction')}
        </Button>
      </ActionBox>
    </HeaderContainer>
  );
};

export default HeaderView;
