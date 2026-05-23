import React, { useState } from 'react';
import ForgotPasswordView from './View';
import { requestPasswordReset } from './api';

/**
 * Forgot Password Page Controller
 * Manages state and API interaction for the password reset flow.
 *
 * @returns {JSX.Element} The rendered Forgot Password page controller.
 */
const ForgotPassword = () => {
  const [email, setEmail] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [formError, setFormError] = useState('');
  const [formSuccess, setFormSuccess] = useState('');

  const handleInputChange = (e) => {
    setEmail(e.target.value);
  };

  const handleFormSubmit = async () => {
    setFormError('');
    setFormSuccess('');

    if (!email) {
      setFormError('Please enter your email address.');
      return;
    }

    try {
      setIsLoading(true);
      const res = await requestPasswordReset({ email });
      
      if (res.status === 200 || res.data) {
        setFormSuccess('If an account exists, a reset link has been sent to your email.');
      } else {
        setFormError('Failed to request password reset. Please try again.');
      }
    } catch (err) {
      if (err?.data?.messages) {
        setFormError(err.data.messages[0]);
      } else {
        setFormError('An unexpected error occurred.');
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <ForgotPasswordView
      email={email}
      handleInputChange={handleInputChange}
      formError={formError}
      formSuccess={formSuccess}
      isLoading={isLoading}
      onSubmit={handleFormSubmit}
    />
  );
};

export default ForgotPassword;
