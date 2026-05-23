import React from 'react';
import { Navigate } from 'react-router-dom';

const PublicRoute = ({ children }) => {
  const token = localStorage.getItem('auth_token');

  if (token) {
    // If the user already has a JWT, redirect them away from public pages (like Login) to the Dashboard.
    return <Navigate to="/" replace />;
  }

  return children;
};

export default PublicRoute;
