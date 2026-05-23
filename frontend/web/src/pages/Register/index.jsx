import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGoogleLogin } from '@react-oauth/google';
import RegisterView from './View';
import { createUser, createUserSSO } from './api';

/**
 * Register Page Controller
 * Manages state, validation, and API interactions for user registration.
 * Connects directly to the RegisterView for presentation.
 *
 * @returns {JSX.Element} The rendered Register page controller.
 */
const Register = () => {
  const navigate = useNavigate();
  const [isGoogleLoading, setIsGoogleLoading] = useState(false);
  const [isFormLoading, setIsFormLoading] = useState(false);
  const [formData, setFormData] = useState({ name: '', email: '', password: '', confirmPassword: '' });
  const [formError, setFormError] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const handleInputChange = (e) => {
    const { id, value } = e.target;
    setFormData((prev) => ({ ...prev, [id]: value }));
  };

  const handleGoogleSignup = useGoogleLogin({
    onSuccess: async (tokenResponse) => {
      try {
        const createRes = await createUserSSO({
          provider_name: 'google',
          provider_access_token: tokenResponse.access_token,
        });

        // In mock mode it will just resolve. Let's redirect to login.
        if (createRes.status === 200 || createRes.status === 201 || createRes.data) {
          navigate('/login');
        } else {
          setFormError('Failed to create account.');
        }
      } catch {
        setFormError('Failed to authenticate with Google.');
      } finally {
        setIsGoogleLoading(false);
      }
    },
    onError: () => {
      setFormError('Failed to authenticate with Google.');
      setIsGoogleLoading(false);
    },
  });

  const handleGoogleButtonClick = () => {
    setIsGoogleLoading(true);
    handleGoogleSignup();
  };

  const handleFormSubmit = async () => {
    setFormError('');
    const { name, email, password, confirmPassword } = formData;

    if (!name || !email || !password || !confirmPassword) {
      setFormError('Please fill in all fields.');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      setFormError('Please enter a valid email address.');
      return;
    }

    if (password !== confirmPassword) {
      setFormError('Passwords do not match.');
      return;
    }

    try {
      setIsFormLoading(true);
      // In mock mode this might just pass
      const createRes = await createUser({ name, email, password });

      if (createRes.status === 200 || createRes.status === 201 || createRes.data) {
        navigate('/login');
      } else {
        setFormError('Failed to create account. Please try again.');
      }
    } catch {
      setFormError('An unexpected error occurred. Please try again.');
    } finally {
      setIsFormLoading(false);
    }
  };

  return (
    <RegisterView
      formData={formData}
      handleInputChange={handleInputChange}
      formError={formError}
      isFormLoading={isFormLoading}
      isGoogleLoading={isGoogleLoading}
      showPassword={showPassword}
      setShowPassword={setShowPassword}
      showConfirmPassword={showConfirmPassword}
      setShowConfirmPassword={setShowConfirmPassword}
      onSubmit={handleFormSubmit}
      onGoogleSignup={handleGoogleButtonClick}
    />
  );
};

export default Register;
