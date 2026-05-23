import React from 'react';
import { Typography, TextField, Button, Box, Alert, Link } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { useText } from '../../hooks/useText';
import LockResetIcon from '@mui/icons-material/LockReset';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { ForgotContainer, ForgotCard, HeaderBox, StyledForm } from './styles';

/**
 * Forgot Password View Template
 * Pure presentation component for the password recovery interface.
 *
 * @param {Object} props - Properties mapping state and handlers from the Controller.
 * @returns {JSX.Element} The rendered visual layer of the Forgot Password page.
 */
const ForgotPasswordView = ({
  email, handleInputChange,
  formError, formSuccess, isLoading,
  onSubmit
}) => {
  const { t } = useText();

  return (
    <ForgotContainer>
      <Box sx={{ position: 'absolute', top: 0, width: '100%', p: 3, display: 'flex', justifyContent: 'space-between', zIndex: 1 }}>
        <Typography variant="h3" sx={{ fontSize: '1.5rem', color: 'primary.main' }}>Nievo Easy Fin</Typography>
      </Box>

      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', width: '100%', maxWidth: 440 }}>
        <ForgotCard>
          <HeaderBox>
            <Box sx={{ display: 'inline-flex', p: 2, borderRadius: '50%', backgroundColor: 'primary.light', mb: 2 }}>
               <LockResetIcon color="primary" sx={{ fontSize: 40 }} />
            </Box>
            <Typography variant="h1" sx={{ fontSize: '2rem', mb: 1, color: 'text.primary' }}>
              {t('ForgotPassword.title')}
            </Typography>
            <Typography variant="body1" color="text.secondary">
              {t('ForgotPassword.description')}
            </Typography>
          </HeaderBox>

          {formError && <Alert severity="error">{formError}</Alert>}
          {formSuccess && <Alert severity="success">{formSuccess}</Alert>}

          <StyledForm>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
              <Typography variant="caption" fontWeight="bold" textTransform="uppercase">{t('ForgotPassword.emailLabel')}</Typography>
              <TextField
                variant="outlined"
                id="email"
                type="email"
                placeholder="you@example.com"
                value={email}
                onChange={handleInputChange}
                required
                disabled={isLoading || formSuccess}
                fullWidth
              />
            </Box>

            <Button
              type="button"
              variant="contained"
              color="primary"
              size="large"
              disabled={isLoading || formSuccess}
              onClick={onSubmit}
              fullWidth
              sx={{ height: 56, mt: 1, fontSize: '1.1rem' }}
            >
              {isLoading ? t('Common.loading') : t('ForgotPassword.submitButton')}
            </Button>
          </StyledForm>

          <Box textAlign="center" mt={3}>
            <Link component={RouterLink} to="/login" color="text.secondary" fontWeight="medium" sx={{ display: 'inline-flex', alignItems: 'center', gap: 1, '&:hover': { color: 'primary.main' } }}>
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
