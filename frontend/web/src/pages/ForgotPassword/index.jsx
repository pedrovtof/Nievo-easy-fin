import React, { useState } from 'react';
import ForgotPasswordView from './View';
import { requestPasswordReset, confirmPasswordReset } from './api';

/**
 * Forgot Password Page Controller
 * Two-step password reset flow:
 *   Step 1: POST { email }                          → backend sends PIN via email
 *   Step 2: PATCH { email, pin_token, password }   → backend resets password
 *
 * State separation is critical: step1Info is shown ONLY between steps,
 * formSuccess is ONLY set after the PATCH succeeds.
 */
const ForgotPassword = () => {
  const [step, setStep] = useState(1);
  const [email, setEmail] = useState('');
  const [pinToken, setPinToken] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [formError, setFormError] = useState('');
  // step1Info: shown as banner at top of step 2 (email sent confirmation)
  const [step1Info, setStep1Info] = useState('');
  // formSuccess: shown only after PATCH succeeds (reset done)
  const [formSuccess, setFormSuccess] = useState('');

  const handleEmailChange = (e) => setEmail(e.target.value);
  const handlePinChange = (e) => setPinToken(e.target.value);
  const handlePasswordChange = (e) => setNewPassword(e.target.value);

  // ── Step 1: Request PIN via email (POST) ──────────────────────────────────
  const handleRequestReset = async () => {
    setFormError('');

    if (!email) {
      setFormError('Please enter your email address.');
      return;
    }

    try {
      setIsLoading(true);
      await requestPasswordReset({ email });
      // Move to step 2 — store the info banner separately, NOT in formSuccess
      setStep1Info(`A PIN was sent to ${email}. Enter it below along with your new password.`);
      setStep(2);
    } catch (err) {
      const messages = err?.response?.data?.messages || err?.data?.messages;
      setFormError(messages?.[0] ?? 'An unexpected error occurred. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  // ── Step 2: Confirm reset with PIN + new password (PATCH) ─────────────────
  const handleConfirmReset = async () => {
    setFormError('');

    if (!pinToken || !newPassword) {
      setFormError('Please enter both the PIN from your email and your new password.');
      return;
    }

    try {
      setIsLoading(true);
      await confirmPasswordReset({ email, pin_token: pinToken, password: newPassword });
      // Only here do we set formSuccess — this hides the form and shows done state
      setFormSuccess('Password reset successfully! You can now log in with your new password.');
    } catch (err) {
      const messages = err?.response?.data?.messages || err?.data?.messages;
      setFormError(messages?.[0] ?? 'Failed to reset password. The PIN may be invalid or expired.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <ForgotPasswordView
      step={step}
      email={email}
      pinToken={pinToken}
      newPassword={newPassword}
      onEmailChange={handleEmailChange}
      onPinChange={handlePinChange}
      onPasswordChange={handlePasswordChange}
      formError={formError}
      step1Info={step1Info}
      formSuccess={formSuccess}
      isLoading={isLoading}
      onRequestReset={handleRequestReset}
      onConfirmReset={handleConfirmReset}
    />
  );
};

export default ForgotPassword;
