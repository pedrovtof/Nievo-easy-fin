import React from 'react';
import { useLocation } from 'react-router-dom';
import SidebarView from './View';

/**
 * Sidebar Navigation Controller
 * Defines the primary navigation links for the authenticated application shell.
 *
 * @returns {JSX.Element} The rendered Sidebar navigation component.
 */
const Sidebar = () => {
  const location = useLocation();
  const navItems = [
    { icon: 'dashboard', label: 'dashboard', path: '/' },
    { icon: 'receipt_long', label: 'transactions', path: '/transactions' },
    { icon: 'account_balance', label: 'banks', path: '/banks' },
    { icon: 'credit_card', label: 'cards', path: '/cards' },
    { icon: 'donut_large', label: 'budget', path: '/budget' },
    { icon: 'category', label: 'categories', path: '/categories' },
    { icon: 'settings', label: 'settings', path: '/settings' },
  ];

  return <SidebarView navItems={navItems} currentPath={location.pathname} />;
};

export default Sidebar;
