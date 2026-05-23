import React from 'react';
import { Typography, IconButton, Button, Box } from '@mui/material';
import NotificationsIcon from '@mui/icons-material/Notifications';
import AddIcon from '@mui/icons-material/Add';
import LogoutIcon from '@mui/icons-material/Logout';
import { HeaderContainer, TitleBox, ActionBox, NotificationBadge } from './styles';
import { useText } from '../../hooks/useText';

const HeaderView = ({ userName, dateString, onNewTransaction, onNotifications, onLogout }) => {
  const { t } = useText();

  return (
    <HeaderContainer position="static">
      <TitleBox>
        <Typography variant="h3" color="primary">
          Good morning, {userName}
        </Typography>
        <Typography variant="subtitle1" color="secondary">
          Here's your financial overview for {dateString}
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
