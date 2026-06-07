import React from 'react';
import {
  Typography, TextField, Button, Box, Alert, Link,
  IconButton, InputAdornment,
} from '@mui/material';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import MarkEmailReadIcon from '@mui/icons-material/MarkEmailRead';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import { Link as RouterLink } from 'react-router-dom';
import { useText } from '../../hooks/useText';
import { RegisterContainer, RegisterCard, HeaderBox, StyledForm, SsoButton } from './styles';
import { DividerBox, DividerText } from '../Login/styles';

/* ── Google Logo SVG ───────────────────────────────────────────────────── */
const GoogleIcon = () => (
  <svg aria-hidden="true" width="20" height="20" viewBox="0 0 24 24">
    <path d="M12.0003 20.45C16.6491 20.45 20.5505 17.2882 22.0003 13.0909H12.0003V10.9091H24.2307C24.4173 11.7582 24.5458 12.78 24.5458 14C24.5458 20.6273 19.1639 26 12.0003 26C5.37302 26 0.000289917 20.6273 0.000289917 14C0.000289917 7.37273 5.37302 2 12.0003 2V6.36364C7.78211 6.36364 4.36393 9.78182 4.36393 14C4.36393 18.2182 7.78211 21.6364 12.0003 21.6364V20.45Z" fill="#EA4335" transform="scale(0.8) translate(3,3)" />
    <path d="M24.2307 10.9091H12.0003V13.0909H22.0003C21.6493 14.2886 21.0526 15.3888 20.2679 16.3273L23.2384 19.2977C23.9457 18.5905 24.3821 17.6545 24.4173 11.7582L24.2307 10.9091Z" fill="#FBBC05" transform="scale(0.8) translate(3,3)" />
    <path d="M4.36393 14C4.36393 11.9695 5.12211 10.1373 6.34938 8.72727L3.37893 5.75682C1.29484 8.01818 0.000289917 10.8841 0.000289917 14H4.36393Z" fill="#34A853" transform="scale(0.8) translate(3,3)" />
    <path d="M12.0003 6.36364C13.8839 6.36364 15.6021 7.02955 16.9458 8.12727L20.0112 5.06182C17.9003 3.09091 15.093 2 12.0003 2V6.36364Z" fill="#4285F4" transform="scale(0.8) translate(3,3)" />
  </svg>
);

/* ══════════════════════════════════════════════════════════════════════════
   RegisterView
   Props:
     step           {1|2}
     -- Step 1 --
     formData, handleInputChange
     formError, isFormLoading, isGoogleLoading
     showPassword, setShowPassword
     showConfirmPassword, setShowConfirmPassword
     onSubmit, onGoogleSignup, onOpenTerms
     -- Step 2 --
     step1Info, pinToken, onPinChange
     isPinLoading, pinError, onPinSubmit
══════════════════════════════════════════════════════════════════════════ */
const RegisterView = ({
  step,
  // step 1
  formData, handleInputChange,
  formError, isFormLoading, isGoogleLoading,
  showPassword, setShowPassword,
  showConfirmPassword, setShowConfirmPassword,
  onSubmit, onGoogleSignup, onOpenTerms,
  // step 2
  step1Info, pinToken, onPinChange,
  isPinLoading, pinError, onPinSubmit,
  // resend
  isResendLoading, resendMessage, resendError, onResend,
}) => {
  const { t } = useText();
  const isSubmitting = isFormLoading || isGoogleLoading;

  return (
    <RegisterContainer>
      {/* Top bar */}
      <Box sx={{ position: 'absolute', top: 0, width: '100%', p: 3, display: 'flex', justifyContent: 'space-between', zIndex: 1 }}>
        <Typography variant="h3" sx={{ fontSize: '1.5rem', color: 'primary.main' }}>Nievo Easy Fin</Typography>
      </Box>

      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '100%', maxWidth: 480 }}>

        {/* ── Step 1: Registration form ─────────────────────────────────── */}
        {step === 1 && (
          <>
            <HeaderBox>
              <Typography variant="h1" sx={{ fontSize: '2.5rem', mb: 1, color: 'text.primary' }}>
                Start your journey
              </Typography>
              <Typography variant="body1" color="text.secondary">
                The Silent Navigator for your family's financial future.
              </Typography>
            </HeaderBox>

            <RegisterCard>
              <SsoButton
                type="button"
                fullWidth
                disabled={isSubmitting}
                onClick={onGoogleSignup}
                startIcon={<GoogleIcon />}
              >
                {isGoogleLoading ? t('Common.loading') : 'Sign up with Google'}
              </SsoButton>

              <DividerBox>
                <DividerText>Or use email</DividerText>
              </DividerBox>

              {formError && <Alert severity="error">{formError}</Alert>}

              <StyledForm>
                {/* Name */}
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                  <Typography variant="caption" fontWeight="bold" textTransform="uppercase">{t('Register.nameLabel')}</Typography>
                  <TextField
                    variant="outlined"
                    id="name"
                    placeholder="Alex Thompson"
                    value={formData.name}
                    onChange={handleInputChange}
                    required
                    disabled={isSubmitting}
                    fullWidth
                  />
                </Box>

                {/* Email */}
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                  <Typography variant="caption" fontWeight="bold" textTransform="uppercase">{t('Register.emailLabel')}</Typography>
                  <TextField
                    variant="outlined"
                    id="email"
                    type="email"
                    placeholder="alex@example.com"
                    value={formData.email}
                    onChange={handleInputChange}
                    required
                    disabled={isSubmitting}
                    fullWidth
                  />
                </Box>

                {/* Passwords */}
                <Box sx={{ display: 'flex', gap: 2 }}>
                  <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5, flex: 1 }}>
                    <Typography variant="caption" fontWeight="bold" textTransform="uppercase">{t('Register.passwordLabel')}</Typography>
                    <TextField
                      variant="outlined"
                      id="password"
                      type={showPassword ? 'text' : 'password'}
                      placeholder="••••••••"
                      value={formData.password}
                      onChange={handleInputChange}
                      required
                      disabled={isSubmitting}
                      fullWidth
                      InputProps={{
                        endAdornment: (
                          <InputAdornment position="end">
                            <IconButton type="button" onClick={() => setShowPassword(!showPassword)} edge="end">
                              {showPassword ? <VisibilityOff /> : <Visibility />}
                            </IconButton>
                          </InputAdornment>
                        ),
                      }}
                    />
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5, flex: 1 }}>
                    <Typography variant="caption" fontWeight="bold" textTransform="uppercase">{t('Register.confirmPasswordLabel')}</Typography>
                    <TextField
                      variant="outlined"
                      id="confirmPassword"
                      type={showConfirmPassword ? 'text' : 'password'}
                      placeholder="••••••••"
                      value={formData.confirmPassword}
                      onChange={handleInputChange}
                      required
                      disabled={isSubmitting}
                      fullWidth
                      InputProps={{
                        endAdornment: (
                          <InputAdornment position="end">
                            <IconButton type="button" onClick={() => setShowConfirmPassword(!showConfirmPassword)} edge="end">
                              {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                            </IconButton>
                          </InputAdornment>
                        ),
                      }}
                    />
                  </Box>
                </Box>

                {/* Terms notice */}
                <Typography variant="caption" color="text.secondary" textAlign="center">
                  By continuing you agree to our{' '}
                  <Link
                    component="button"
                    type="button"
                    onClick={onOpenTerms}
                    color="primary.main"
                    fontWeight="bold"
                    sx={{ verticalAlign: 'baseline', cursor: 'pointer', textDecoration: 'underline' }}
                  >
                    terms of use
                  </Link>
                </Typography>

                <Button
                  type="button"
                  variant="contained"
                  color="primary"
                  size="large"
                  disabled={isSubmitting}
                  onClick={onSubmit}
                  fullWidth
                  sx={{ height: 56, mt: 1, fontSize: '1.1rem' }}
                >
                  {isFormLoading ? t('Common.loading') : t('Register.registerButton')}
                </Button>
              </StyledForm>

              <Box textAlign="center" mt={2} sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                <Typography variant="body2" color="text.secondary">
                  Need only to confirm the email?{' '}
                  <Link component={RouterLink} to="/confirm-email" color="tertiary.main" fontWeight="bold">
                    Click here
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
          </>
        )}

        {/* ── Step 2: Email PIN verification ───────────────────────────── */}
        {step === 2 && (
          <>
            <HeaderBox>
              <Typography variant="h1" sx={{ fontSize: '2.5rem', mb: 1, color: 'text.primary' }}>
                Verify your email
              </Typography>
              <Typography variant="body1" color="text.secondary">
                Enter the verification code we sent to your inbox to confirm your account.
              </Typography>
            </HeaderBox>

            <RegisterCard>
              {/* Email icon */}
              <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                <Box sx={{ display: 'inline-flex', p: 2, borderRadius: '50%', backgroundColor: 'primary.light' }}>
                  <MarkEmailReadIcon color="primary" sx={{ fontSize: 48 }} />
                </Box>
              </Box>

              {/* Info banner from step 1 */}
              {step1Info && <Alert severity="info">{step1Info}</Alert>}

              {/* PIN error */}
              {pinError && <Alert severity="error">{pinError}</Alert>}

              <StyledForm>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                  <Typography variant="caption" fontWeight="bold" textTransform="uppercase">
                    Verification code
                  </Typography>
                  <TextField
                    variant="outlined"
                    id="reg-pin"
                    type="text"
                    placeholder="ex: 694573"
                    value={pinToken}
                    onChange={onPinChange}
                    required
                    disabled={isPinLoading}
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
                  disabled={isPinLoading || isResendLoading}
                  onClick={onPinSubmit}
                  fullWidth
                  sx={{ height: 56, mt: 1, fontSize: '1.1rem' }}
                >
                  {isPinLoading ? t('Common.loading') : 'Confirm account'}
                </Button>

                {/* Resend feedback */}
                {resendMessage && <Alert severity="success">{resendMessage}</Alert>}
                {resendError && <Alert severity="error">{resendError}</Alert>}

                <Button
                  type="button"
                  variant="outlined"
                  color="primary"
                  size="large"
                  disabled={isPinLoading || isResendLoading}
                  onClick={onResend}
                  fullWidth
                  sx={{ height: 48, fontSize: '0.95rem' }}
                >
                  {isResendLoading ? t('Common.loading') : 'Resend verification code'}
                </Button>
              </StyledForm>

              <Box textAlign="center" mt={2}>
                <Typography variant="body2" color="text.secondary">
                  Already have an account?{' '}
                  <Link component={RouterLink} to="/login" color="tertiary.main" fontWeight="bold">
                    Login
                  </Link>
                </Typography>
              </Box>
            </RegisterCard>
          </>
        )}
      </Box>
    </RegisterContainer>
  );
};

export default RegisterView;
