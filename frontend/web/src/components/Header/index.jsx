import React from 'react';
import HeaderView from './View';

const Header = () => {
  // Logic could involve fetching user data or notifications
  const handleNewTransaction = () => {
    console.log('New Transaction Clicked');
  };

  const handleNotifications = () => {
    console.log('Notifications Clicked');
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
    />
  );
};

export default Header;
