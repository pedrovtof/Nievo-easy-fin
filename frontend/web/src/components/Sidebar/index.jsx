import React from 'react';
import { useLocation } from 'react-router-dom';
import SidebarView from './View';

const Sidebar = () => {
  const location = useLocation();
  const navItems = [
    { icon: 'dashboard', label: 'dashboard', path: '/' },
    { icon: 'receipt_long', label: 'transactions', path: '/transactions' },
    { icon: 'donut_large', label: 'budget', path: '/budget' },
    { icon: 'settings', label: 'settings', path: '/settings' },
  ];

  return <SidebarView navItems={navItems} currentPath={location.pathname} />;
};

export default Sidebar;
