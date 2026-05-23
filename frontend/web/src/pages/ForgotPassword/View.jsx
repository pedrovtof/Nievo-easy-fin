import React from 'react';
import {
  Typography, TextField, Button, Box, Alert, Link, InputAdornment, IconButton,
} from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { useText } from '../../hooks/useText';
import LockResetIcon from '@mui/icons-material/LockReset';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import { ForgotContainer, ForgotCard, HeaderBox, StyledForm } from './styles';

/**
 * Forgot Password View — two-step password recovery
 *
 * step === 1  →  Email form (request PIN)
 * step === 2  →  PIN + new password form (confirm reset)
 *              →  Success screen after PATCH succeeds (formSuccess set)
 *
 * Props:
 *   step          {number}   1 or 2
 *   email         {string}
 *   pinToken      {string}
 *   newPassword   {string}
 *   onEmailChange    {fn}
 *   onPinChange      {fn}
 *   onPasswordChange {fn}
 *   formError     {string}   error to show in current step
 *   step1Info     {string}   info banner shown at top of step 2 (PIN sent message)
 *   formSuccess   {string}   set ONLY after PATCH succeeds → hides form, shows done screen
 *   isLoading     {boolean}
 *   onRequestReset  {fn}
 *   onConfirmReset  {fn}
 */
const ForgotPasswordView = ({
  step,
  email, onEmailChange,
  pinToken, onPinChange,
  newPassword, onPasswordChange,
  formError,
  step1Info,
  formSuccess,
  isLoading,
  onRequestReset,
  onConfirmReset,
}) => {
  const { t } = useText();

  // ── Success screen (after PATCH) ────────────────────────────────────────────
  if (formSuccess) {
    return (
      <ForgotContainer>
        <Box sx={{ position: 'absolute', top: 0, width: '100%', p: 3, zIndex: 1 }}>
          <Typography variant="h3" sx={{ fontSize: '1.5rem', color: 'primary.main' }}>Nievo Easy Fin</Typography>
        </Box>
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '100%', maxWidth: 440 }}>
          <ForgotCard>
            <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2, textAlign: 'center' }}>
              <Box sx={{ display: 'inline-flex', p: 2, borderRadius: '50%', backgroundColor: 'success.light', mb: 1 }}>
                <CheckCircleOutlineIcon sx={{ fontSize: 48, color: 'success.main' }} />
              </Box>
              <Typography variant="h2" color="text.primary">Password Reset!</Typography>
              <Typography variant="body1" color="text.secondary">{formSuccess}</Typography>
              <Button
                component={RouterLink}
                to="/login"
                variant="contained"
                color="primary"
                size="large"
                fullWidth
                sx={{ height: 56, mt: 2, fontSize: '1.1rem' }}
              >
                Go to Login
              </Button>
            </Box>
          </ForgotCard>
        </Box>
      </ForgotContainer>
    );
  }

  return (
    <ForgotContainer>
      <Box sx={{ position: 'absolute', top: 0, width: '100%', p: 3, zIndex: 1 }}>
        <Typography variant="h3" sx={{ fontSize: '1.5rem', color: 'primary.main' }}>Nievo Easy Fin</Typography>
      </Box>

      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '100%', maxWidth: 440 }}>
        <ForgotCard>
          {/* Header */}
          <HeaderBox>
            <Box sx={{ display: 'inline-flex', p: 2, borderRadius: '50%', backgroundColor: 'primary.light', mb: 2 }}>
              <LockResetIcon color="primary" sx={{ fontSize: 40 }} />
            </Box>
            <Typography variant="h1" sx={{ fontSize: '2rem', mb: 1, color: 'text.primary' }}>
              {step === 1 ? t('ForgotPassword.title') : 'Check your email'}
            </Typography>
            <Typography variant="body1" color="text.secondary">
              {step === 1
                ? t('ForgotPassword.description')
                : 'Enter the PIN code from your email and choose a new password.'}
            </Typography>
          </HeaderBox>

          {/* Banners */}
          {step1Info && <Alert severity="info" sx={{ mb: 1 }}>{step1Info}</Alert>}
          {formError && <Alert severity="error">{formError}</Alert>}

          {/* ── Step 1: Email ────────────────────────────────────────────── */}
          {step === 1 && (
            <StyledForm>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                <Typography variant="caption" fontWeight="bold" textTransform="uppercase">
                  {t('ForgotPassword.emailLabel')}
                </Typography>
                <TextField
                  variant="outlined"
                  id="fp-email"
                  type="email"
                  placeholder="you@example.com"
                  value={email}
                  onChange={onEmailChange}
                  required
                  disabled={isLoading}
                  fullWidth
                  autoFocus
                />
              </Box>

              <Button
                type="button"
                variant="contained"
                color="primary"
                size="large"
                disabled={isLoading}
                onClick={onRequestReset}
                fullWidth
                sx={{ height: 56, mt: 1, fontSize: '1.1rem' }}
              >
                {isLoading ? t('Common.loading') : t('ForgotPassword.submitButton')}
              </Button>
            </StyledForm>
          )}

          {/* ── Step 2: PIN + New Password ──────────────────────────────── */}
          {step === 2 && (
            <StyledForm>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                <Typography variant="caption" fontWeight="bold" textTransform="uppercase">
                  PIN Code
                </Typography>
                <TextField
                  variant="outlined"
                  id="fp-pin"
                  type="text"
                  placeholder="e.g. 694573"
                  value={pinToken}
                  onChange={onPinChange}
                  required
                  disabled={isLoading}
                  fullWidth
                  autoFocus
                  inputProps={{ maxLength: 10, inputMode: 'numeric' }}
                />
              </Box>

              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                <Typography variant="caption" fontWeight="bold" textTransform="uppercase">
                  New Password
                </Typography>
                <TextField
                  variant="outlined"
                  id="fp-new-password"
                  type="password"
                  placeholder="••••••••"
                  value={newPassword}
                  onChange={onPasswordChange}
                  required
                  disabled={isLoading}
                  fullWidth
                />
              </Box>

              <Button
                type="button"
                variant="contained"
                color="primary"
                size="large"
                disabled={isLoading}
                onClick={onConfirmReset}
                fullWidth
                sx={{ height: 56, mt: 1, fontSize: '1.1rem' }}
              >
                {isLoading ? t('Common.loading') : 'Reset Password'}
              </Button>
            </StyledForm>
          )}

          {/* Back to login */}
          <Box textAlign="center" mt={3}>
            <Link
              component={RouterLink}
              to="/login"
              color="text.secondary"
              fontWeight="medium"
              sx={{ display: 'inline-flex', alignItems: 'center', gap: 1, '&:hover': { color: 'primary.main' } }}
            >
              <ArrowBackIcon fontSize="small" />
              {t('ForgotPassword.backToLogin')}
            </Link>
          </Box>
        </ForgotCard>
      </Box>
    </ForgotContainer>
  );
};

export default ForgotPasswordView;
