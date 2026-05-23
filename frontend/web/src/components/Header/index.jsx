import React from 'react';
import { useNavigate } from 'react-router-dom';
import HeaderView from './View';

const Header = () => {
  const navigate = useNavigate();

  // Logic could involve fetching user data or notifications
  const handleNewTransaction = () => {
    console.log('New Transaction Clicked');
  };

  const handleNotifications = () => {
    console.log('Notifications Clicked');
  };

  const handleLogout = () => {
    localStorage.removeItem('auth_token');
    navigate('/login');
  };

  const today = new Date().toLocaleDateString('en-US', {
    month: 'long',
    day: 'numeric',
    year: 'numeric',
  });

  return (
    <HeaderView
      userName="Alex"
      dateString={today}
      onNewTransaction={handleNewTransaction}
      onNotifications={handleNotifications}
      onLogout={handleLogout}
    />
  );
};

export default Header;
