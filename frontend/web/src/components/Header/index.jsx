import React from 'react';
import { useNavigate } from 'react-router-dom';
import HeaderView from './View';

/**
 * Header Navigation Controller
 * Handles top-level quick actions such as navigating to transaction creation and logging out.
 *
 * @returns {JSX.Element} The rendered Header component.
 */
const Header = () => {
  const navigate = useNavigate();

  const handleNewTransaction = () => {
    navigate('/transactions');
  };

  const handleNotifications = () => {
    navigate('/budget');
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
