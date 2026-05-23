import React from 'react';
import { Typography, TextField, Button, Box, Alert, Link } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { useText } from '../../hooks/useText';
import { RegisterContainer, RegisterCard, HeaderBox, StyledForm, SsoButton } from './styles';
import { DividerBox, DividerText } from '../Login/styles';

const GoogleIcon = () => (
  <svg aria-hidden="true" width="20" height="20" viewBox="0 0 24 24">
    <path d="M12.0003 20.45C16.6491 20.45 20.5505 17.2882 22.0003 13.0909H12.0003V10.9091H24.2307C24.4173 11.7582 24.5458 12.78 24.5458 14C24.5458 20.6273 19.1639 26 12.0003 26C5.37302 26 0.000289917 20.6273 0.000289917 14C0.000289917 7.37273 5.37302 2 12.0003 2V6.36364C7.78211 6.36364 4.36393 9.78182 4.36393 14C4.36393 18.2182 7.78211 21.6364 12.0003 21.6364V20.45Z" fill="#EA4335" transform="scale(0.8) translate(3,3)" />
    <path d="M24.2307 10.9091H12.0003V13.0909H22.0003C21.6493 14.2886 21.0526 15.3888 20.2679 16.3273L23.2384 19.2977C23.9457 18.5905 24.3821 17.6545 24.4173 11.7582L24.2307 10.9091Z" fill="#FBBC05" transform="scale(0.8) translate(3,3)" />
    <path d="M4.36393 14C4.36393 11.9695 5.12211 10.1373 6.34938 8.72727L3.37893 5.75682C1.29484 8.01818 0.000289917 10.8841 0.000289917 14H4.36393Z" fill="#34A853" transform="scale(0.8) translate(3,3)" />
    <path d="M12.0003 6.36364C13.8839 6.36364 15.6021 7.02955 16.9458 8.12727L20.0112 5.06182C17.9003 3.09091 15.093 2 12.0003 2V6.36364Z" fill="#4285F4" transform="scale(0.8) translate(3,3)" />
  </svg>
);

const RegisterView = ({
  formData, handleInputChange,
  formError, isFormLoading, isGoogleLoading,
  onSubmit, onGoogleSignup
}) => {
  const { t } = useText();
  const isSubmitting = isFormLoading || isGoogleLoading;

  return (
    <RegisterContainer>
      <Box sx={{ position: 'absolute', top: 0, width: '100%', p: 3, display: 'flex', justifyContent: 'space-between', zIndex: 1 }}>
        <Typography variant="h3" sx={{ fontSize: '1.5rem', color: 'primary.main' }}>Nievo Easy Fin</Typography>
      </Box>

      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '100%', maxWidth: 480 }}>
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

          {formError && (
            <Alert severity="error">{formError}</Alert>
          )}

          <StyledForm>
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

            <Box sx={{ display: 'flex', gap: 2 }}>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5, flex: 1 }}>
                <Typography variant="caption" fontWeight="bold" textTransform="uppercase">{t('Register.passwordLabel')}</Typography>
                <TextField
                  variant="outlined"
                  id="password"
                  type="password"
                  placeholder="••••••••"
                  value={formData.password}
                  onChange={handleInputChange}
                  required
                  disabled={isSubmitting}
                  fullWidth
                />
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5, flex: 1 }}>
                <Typography variant="caption" fontWeight="bold" textTransform="uppercase">{t('Register.confirmPasswordLabel')}</Typography>
                <TextField
                  variant="outlined"
                  id="confirmPassword"
                  type="password"
                  placeholder="••••••••"
                  value={formData.confirmPassword}
                  onChange={handleInputChange}
                  required
                  disabled={isSubmitting}
                  fullWidth
                />
              </Box>
            </Box>

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

          <Box textAlign="center" mt={2}>
            <Typography variant="body2" color="text.secondary">
              {t('Register.loginLink').split('?')[0]}?{' '}
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

export default RegisterView;
