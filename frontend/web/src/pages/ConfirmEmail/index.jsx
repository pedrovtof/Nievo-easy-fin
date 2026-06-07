import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Typography, TextField, Button, Box, Alert, Link, CircularProgress,
} from '@mui/material';
import MarkEmailReadIcon from '@mui/icons-material/MarkEmailRead';
import { Link as RouterLink } from 'react-router-dom';
import { validateEmailToken, sendValidateEmail } from '../Register/api';
import { RegisterContainer, RegisterCard, HeaderBox, StyledForm } from '../Register/styles';

/**
 * Confirm Email Page
 * A standalone route (/confirm-email) that lets users who have already
 * registered but not yet verified their email to do so at any time.
 *
 * The user enters:
 *   1. Their email address (so we know which account to validate)
 *   2. The PIN code received in their inbox
 */
const ConfirmEmail = () => {
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [pinToken, setPinToken] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  const [isResendLoading, setIsResendLoading] = useState(false);
  const [resendMessage, setResendMessage] = useState('');
  const [resendError, setResendError] = useState('');

  const handleSubmit = async () => {
    setError('');

    if (!email.trim()) {
      setError('Please enter your email address.');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      setError('Please enter a valid email address.');
      return;
    }

    if (!pinToken.trim()) {
      setError('Please enter the verification code sent to your email.');
      return;
    }

    const parsed = parseInt(pinToken, 10);
    if (isNaN(parsed)) {
      setError('The verification code must contain numbers only.');
      return;
    }

    try {
      setIsLoading(true);
      await validateEmailToken({ email: email.trim(), pin_token: parsed });
      setSuccess(true);
      // Redirect to login after a short delay so the user sees the success state
      setTimeout(() => navigate('/login'), 2500);
    } catch (err) {
      if (!err?.response) {
        setError('Unable to reach the server. Please check your connection and try again.');
      } else {
        const messages = err?.response?.data?.messages || err?.data?.messages;
        setError(
          messages?.[0] ||
          `Verification failed (status ${err.response.status}). The code may be invalid or expired. Please try again.`,
        );
      }
    } finally {
      setIsLoading(false);
    }
  };

  const handleResend = async () => {
    setResendMessage('');
    setResendError('');

    if (!email.trim()) {
      setResendError('Please enter your email address first.');
      return;
    }

    try {
      setIsResendLoading(true);
      await sendValidateEmail({ email: email.trim() });
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

  return (
    <RegisterContainer>
      {/* Top bar */}
      <Box sx={{ position: 'absolute', top: 0, width: '100%', p: 3, display: 'flex', justifyContent: 'space-between', zIndex: 1 }}>
        <Typography variant="h3" sx={{ fontSize: '1.5rem', color: 'primary.main' }}>Nievo Easy Fin</Typography>
      </Box>

      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '100%', maxWidth: 480 }}>
        <HeaderBox>
          <Typography variant="h1" sx={{ fontSize: '2.5rem', mb: 1, color: 'text.primary' }}>
            Verify your email
          </Typography>
          <Typography variant="body1" color="text.secondary">
            Enter your email and the verification code we sent to your inbox.
          </Typography>
        </HeaderBox>

        <RegisterCard>
          {/* Email icon */}
          <Box sx={{ display: 'flex', justifyContent: 'center' }}>
            <Box sx={{ display: 'inline-flex', p: 2, borderRadius: '50%', backgroundColor: 'primary.light' }}>
              <MarkEmailReadIcon color="primary" sx={{ fontSize: 48 }} />
            </Box>
          </Box>

          {/* Success state */}
          {success && (
            <Alert severity="success" icon={false} sx={{ textAlign: 'center', fontWeight: 'bold' }}>
              ✅ Your account has been confirmed! Redirecting to login…
            </Alert>
          )}

          {/* Error state */}
          {error && <Alert severity="error">{error}</Alert>}

          {!success && (
            <StyledForm>
              {/* Email field */}
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                <Typography variant="caption" fontWeight="bold" textTransform="uppercase">
                  Email address
                </Typography>
                <TextField
                  variant="outlined"
                  id="confirm-email-address"
                  type="email"
                  placeholder="alex@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  disabled={isLoading}
                  fullWidth
                />
              </Box>

              {/* PIN field */}
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                <Typography variant="caption" fontWeight="bold" textTransform="uppercase">
                  Verification code
                </Typography>
                <TextField
                  variant="outlined"
                  id="confirm-email-pin"
                  type="text"
                  placeholder="e.g. 694573"
                  value={pinToken}
                  onChange={(e) => setPinToken(e.target.value)}
                  required
                  disabled={isLoading}
                  fullWidth
                  autoFocus
                  inputProps={{ maxLength: 10, inputMode: 'numeric' }}
                />
              </Box>

              <Button
                type="button"
                variant="contained"
                color="primary"
                size="large"
                disabled={isLoading || isResendLoading}
                onClick={handleSubmit}
                fullWidth
                sx={{ height: 56, mt: 1, fontSize: '1.1rem' }}
              >
                {isLoading ? <CircularProgress size={24} color="inherit" /> : 'Confirm account'}
              </Button>

              {/* Resend feedback */}
              {resendMessage && <Alert severity="success">{resendMessage}</Alert>}
              {resendError && <Alert severity="error">{resendError}</Alert>}

              <Button
                type="button"
                variant="outlined"
                color="primary"
                size="large"
                disabled={isLoading || isResendLoading}
                onClick={handleResend}
                fullWidth
                sx={{ height: 48, fontSize: '0.95rem' }}
              >
                {isResendLoading ? <CircularProgress size={20} color="inherit" /> : 'Resend verification code'}
              </Button>
            </StyledForm>
          )}

          <Box textAlign="center" mt={2} sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
            <Typography variant="body2" color="text.secondary">
              Don&apos;t have an account yet?{' '}
              <Link component={RouterLink} to="/register" color="tertiary.main" fontWeight="bold">
                Sign up
              </Link>
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Already have an account?{' '}
              <Link component={RouterLink} to="/login" color="tertiary.main" fontWeight="bold">
                Login
              </Link>
            </Typography>
          </Box>
        </RegisterCard>
      </Box>
    </RegisterContainer>
  );
};

export default ConfirmEmail;
