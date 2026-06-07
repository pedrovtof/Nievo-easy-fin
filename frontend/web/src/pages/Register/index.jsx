import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGoogleLogin } from '@react-oauth/google';
import RegisterView from './View';
import { createUser, createUserSSO, validateEmailToken, sendValidateEmail } from './api';

/**
 * Register Page Controller
 * Manages a two-step registration flow:
 *   Step 1: User fills name, email, password → POST /singup (accept_terms=true)
 *           Backend creates user with INVALID status and sends PIN to email
 *   Step 2: User enters PIN received in email → POST /validate:email
 *           Backend activates the account (status = ACTIVE)
 *
 * Also handles the "accept terms" link that opens a new browser tab with the
 * terms content returned by GET /accept-terms:singup.
 */
const Register = ({ initialStep = 1, initialEmail = '' }) => {
  const navigate = useNavigate();

  // ── Steps ─────────────────────────────────────────────────────────────────
  // step 1 = registration form, step 2 = PIN validation
  const [step, setStep] = useState(initialStep);

  // ── Step 1 state ──────────────────────────────────────────────────────────
  const [isGoogleLoading, setIsGoogleLoading] = useState(false);
  const [isFormLoading, setIsFormLoading] = useState(false);
  const [formData, setFormData] = useState({ name: '', email: initialEmail, password: '', confirmPassword: '' });
  const [formError, setFormError] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  // ── Step 2 state ──────────────────────────────────────────────────────────
  const [pinToken, setPinToken] = useState('');
  const [isPinLoading, setIsPinLoading] = useState(false);
  const [pinError, setPinError] = useState('');
  const [step1Info, setStep1Info] = useState('');

  // ── Resend token state ────────────────────────────────────────────────────
  const [isResendLoading, setIsResendLoading] = useState(false);
  const [resendMessage, setResendMessage] = useState('');
  const [resendError, setResendError] = useState('');

  // ── Handlers ──────────────────────────────────────────────────────────────
  const handleInputChange = (e) => {
    const { id, value } = e.target;
    setFormData((prev) => ({ ...prev, [id]: value }));
  };

  const handlePinChange = (e) => setPinToken(e.target.value);

  // ── Resend verification token ─────────────────────────────────────────────
  const handleResend = async () => {
    setResendMessage('');
    setResendError('');
    try {
      setIsResendLoading(true);
      await sendValidateEmail({ email: formData.email });
      setResendMessage('A new verification code has been sent to your email.');
    } catch (err) {
      if (!err?.response) {
        setResendError('Unable to reach the server. Please check your connection and try again.');
      } else {
        const messages = err?.response?.data?.messages || err?.data?.messages;
        setResendError(messages?.[0] || 'Failed to resend the verification code. Please try again.');
      }
    } finally {
      setIsResendLoading(false);
    }
  };

  // Open the branded /terms page in a new browser tab.
  // The Terms page itself fetches the content from the API with full design system styling.
  const handleOpenTerms = () => {
    window.open('/terms', '_blank', 'noopener,noreferrer');
  };

  // ── Google SSO ────────────────────────────────────────────────────────────
  const handleGoogleSignup = useGoogleLogin({
    onSuccess: async (tokenResponse) => {
      try {
        const createRes = await createUserSSO({
          provider_name: 'google',
          provider_access_token: tokenResponse.access_token,
          accept_terms: true,
        });

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

  // ── Step 1: Submit registration form ──────────────────────────────────────
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
      await createUser({ name, email, password, accept_terms: true });
      // Backend sends a PIN to the user's email; move to step 2
      setStep1Info(`A verification code has been sent to ${email}. Enter it below to activate your account.`);
      setStep(2);
    } catch (err) {
      const messages = err?.response?.data?.messages || err?.data?.messages;
      setFormError(messages?.[0] ?? 'An unexpected error occurred. Please try again.');
    } finally {
      setIsFormLoading(false);
    }
  };

  // ── Step 2: Validate PIN token ─────────────────────────────────────────────
  const handlePinSubmit = async () => {
    setPinError('');

    if (!pinToken.trim()) {
      setPinError('Please enter the verification code sent to your email.');
      return;
    }

    const parsed = parseInt(pinToken, 10);
    if (isNaN(parsed)) {
      setPinError('The verification code must contain numbers only.');
      return;
    }

    try {
      setIsPinLoading(true);
      await validateEmailToken({ email: formData.email, pin_token: parsed });
      navigate('/login');
    } catch (err) {
      // Always show a clear error — never silently fail or reload
      if (!err?.response) {
        // Network error — no response from server
        setPinError('Unable to reach the server. Please check your connection and try again.');
      } else {
        const messages = err?.response?.data?.messages || err?.data?.messages;
        setPinError(
          messages?.[0] ||
          `Verification failed (status ${err.response.status}). The code may be invalid or expired. Please try again.`
        );
      }
    } finally {
      setIsPinLoading(false);
    }
  };

  return (
    <RegisterView
      // Step control
      step={step}
      // Step 1 props
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
      onOpenTerms={handleOpenTerms}
      // Step 2 props
      step1Info={step1Info}
      pinToken={pinToken}
      onPinChange={handlePinChange}
      isPinLoading={isPinLoading}
      pinError={pinError}
      onPinSubmit={handlePinSubmit}
      // Resend token props
      isResendLoading={isResendLoading}
      resendMessage={resendMessage}
      resendError={resendError}
      onResend={handleResend}
    />
  );
};

export default Register;
