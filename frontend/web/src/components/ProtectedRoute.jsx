import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';

/**
 * Protected Route Guard
 * Intercepts routing to check for an active user session (JWT).
 * Redirects unauthenticated users to the Login page.
 *
 * @param {Object} props
 * @param {JSX.Element} props.children - The protected component to render if authenticated.
 * @returns {JSX.Element} The children or a Navigate redirect.
 */
const ProtectedRoute = ({ children }) => {
  const token = localStorage.getItem('auth_token');
  const location = useLocation();

  if (!token) {
    // Redirect them to the /login page, but save the current location they were
    // trying to go to when they were redirected. This allows us to send them
    // along to that page after they login, which is a nicer user experience
    // than dropping them off on the home page.
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
};

export default ProtectedRoute;
