import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGoogleLogin } from '@react-oauth/google';
import LoginView from './View';
// The auth API functions are correctly imported if they exist in services/api
// Assuming they do since the original used them. If mock is enabled, it intercepts them.
import { loginUser, loginUserSSO } from './api';

/**
 * Login Page Controller
 * Manages state, validation, and API interactions for user authentication.
 *
 * @returns {JSX.Element} The rendered Login page controller.
 */
const Login = () => {
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [errors, setErrors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [ssoLoading, setSsoLoading] = useState(false);

  const handleAuthSuccess = (token) => {
    localStorage.setItem('auth_token', token);
    navigate('/');
  };

  const handleApiError = (response, err) => {
    if (response?.data?.error && Array.isArray(response.data.messages)) {
      setErrors(response.data.messages);
    } else if (err?.response?.data?.messages) {
      setErrors(err.response.data.messages);
    } else {
      setErrors(['An unexpected error occurred. Please try again.']);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors([]);
    setLoading(true);

    try {
      // In mock mode, this hits the mock interceptor
      // When VITE_USE_MOCK=true, api.js intercepts
      const response = await loginUser({ email, password });
      
      // Adapt to mock data structure
      const data = response.data || response; // interceptor might return directly
      
      if (data.token || (data.data && data.data.token)) {
        handleAuthSuccess(data.token || data.data.token);
      } else if (data.success && data.data?.token) {
        handleAuthSuccess(data.data.token);
      } else {
        handleApiError(response, null);
      }
    } catch (err) {
      handleApiError(null, err);
    } finally {
      setLoading(false);
    }
  };

  const handleGoogleLogin = useGoogleLogin({
    onSuccess: async (tokenResponse) => {
      setSsoLoading(true);
      setErrors([]);

      try {
        const response = await loginUserSSO({
          provider_name: 'google',
          provider_access_token: tokenResponse.access_token,
        });
        const data = response.data || response;

        if (data.token || (data.data && data.data.token)) {
          handleAuthSuccess(data.token || data.data.token);
        } else if (data.success && data.data?.token) {
          handleAuthSuccess(data.data.token);
        } else {
          handleApiError(response, null);
        }
      } catch (err) {
        handleApiError(null, err);
      } finally {
        setSsoLoading(false);
      }
    },
    onError: () => {
      setErrors(['Failed to authenticate with Google.']);
    },
  });

  return (
    <LoginView
      email={email}
      setEmail={setEmail}
      password={password}
      setPassword={setPassword}
      showPassword={showPassword}
      setShowPassword={setShowPassword}
      errors={errors}
      loading={loading}
      ssoLoading={ssoLoading}
      onSubmit={handleSubmit}
      onGoogleLogin={handleGoogleLogin}
    />
  );
};

export default Login;
