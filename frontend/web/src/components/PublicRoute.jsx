import React from 'react';
import { Navigate } from 'react-router-dom';

/**
 * Public Route Guard
 * Intercepts routing for authentication pages (Login, Register).
 * Redirects already authenticated users to the Dashboard.
 *
 * @param {Object} props
 * @param {JSX.Element} props.children - The public component to render.
 * @returns {JSX.Element} The children or a Navigate redirect.
 */
const PublicRoute = ({ children }) => {
  const token = localStorage.getItem('auth_token');

  if (token) {
    // If the user already has a JWT, redirect them away from public pages (like Login) to the Dashboard.
    return <Navigate to="/" replace />;
  }

  return children;
};

export default PublicRoute;
