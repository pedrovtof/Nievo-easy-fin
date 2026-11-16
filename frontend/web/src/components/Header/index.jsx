import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import HeaderView from './View';
import { getUserDetails } from './api';

/**
 * Extracts user details object safely from the Core API response envelope.
 *
 * @param {Object} response - Raw Axios response object.
 * @returns {Object|null} Extracted user details ({ name, email, phone, status }) or null.
 */
export const extractHeaderUserDetails = (response) => {
  const payload = response?.data?.data ?? response?.data ?? null;
  if (payload && typeof payload === 'object' && (payload.name || payload.email)) {
    return payload;
  }
  return null;
};

/**
 * Header Navigation Controller
 * Handles top-level quick actions, displays authenticated user details, and manages logout.
 *
 * @returns {JSX.Element} The rendered Header component.
 */
const Header = () => {
  const navigate = useNavigate();
  const [userName, setUserName] = useState('User');
  const [userStatus, setUserStatus] = useState('');

  useEffect(() => {
    let isMounted = true;
    const fetchUser = async () => {
      try {
        const res = await getUserDetails();
        const details = extractHeaderUserDetails(res);
        if (isMounted && details) {
          setUserName(details.name || details.email || 'User');
          setUserStatus(details.status || '');
        }
      } catch {
        // Fallback silently to default greeting when unauthenticated or offline
      }
    };
    fetchUser();
    return () => {
      isMounted = false;
    };
  }, []);

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
      userName={userName}
      userStatus={userStatus}
      dateString={today}
      onNewTransaction={handleNewTransaction}
      onNotifications={handleNotifications}
      onLogout={handleLogout}
    />
  );
};

export default Header;
