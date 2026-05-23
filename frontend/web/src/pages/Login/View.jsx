import React from 'react';
import { Typography, TextField, Button, Box, Alert, IconButton, InputAdornment, Link } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import MailOutlineIcon from '@mui/icons-material/MailOutline';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { useText } from '../../hooks/useText';
import { LoginContainer, LoginCard, LogoHeader, StyledForm, DividerBox, DividerText, SsoButton } from './styles';

const GoogleIcon = () => (
  <svg aria-hidden="true" width="20" height="20" viewBox="0 0 24 24">
    <path d="M12.0003 20.45C16.6491 20.45 20.5505 17.2882 22.0003 13.0909H12.0003V10.9091H24.2307C24.4173 11.7582 24.5458 12.78 24.5458 14C24.5458 20.6273 19.1639 26 12.0003 26C5.37302 26 0.000289917 20.6273 0.000289917 14C0.000289917 7.37273 5.37302 2 12.0003 2V6.36364C7.78211 6.36364 4.36393 9.78182 4.36393 14C4.36393 18.2182 7.78211 21.6364 12.0003 21.6364V20.45Z" fill="#EA4335" transform="scale(0.8) translate(3,3)" />
    <path d="M24.2307 10.9091H12.0003V13.0909H22.0003C21.6493 14.2886 21.0526 15.3888 20.2679 16.3273L23.2384 19.2977C23.9457 18.5905 24.3821 17.6545 24.4173 11.7582L24.2307 10.9091Z" fill="#FBBC05" transform="scale(0.8) translate(3,3)" />
    <path d="M4.36393 14C4.36393 11.9695 5.12211 10.1373 6.34938 8.72727L3.37893 5.75682C1.29484 8.01818 0.000289917 10.8841 0.000289917 14H4.36393Z" fill="#34A853" transform="scale(0.8) translate(3,3)" />
    <path d="M12.0003 6.36364C13.8839 6.36364 15.6021 7.02955 16.9458 8.12727L20.0112 5.06182C17.9003 3.09091 15.093 2 12.0003 2V6.36364Z" fill="#4285F4" transform="scale(0.8) translate(3,3)" />
  </svg>
);

const LoginView = ({
  email, setEmail,
  password, setPassword,
  showPassword, setShowPassword,
  errors, loading, ssoLoading,
  onSubmit, onGoogleLogin
}) => {
  const { t } = useText();
  const isSubmitting = loading || ssoLoading;

  return (
    <LoginContainer>
      <LoginCard>
        <LogoHeader>
          <Box sx={{ width: 48, height: 48, bgcolor: 'surface.high', borderRadius: 2, display: 'flex', alignItems: 'center', justifyContent: 'center', mb: 1.5 }}>
            <AccountBalanceWalletIcon color="primary" sx={{ fontSize: 32 }} />
          </Box>
          <Typography variant="h2" color="text.primary">Nievo Easy Fin</Typography>
          <Typography variant="body2" color="text.secondary">Focus & Clarity for your finances</Typography>
        </LogoHeader>

        {errors.length > 0 && (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
            {errors.map((msg, idx) => (
              <Alert severity="error" key={idx}>{msg}</Alert>
            ))}
          </Box>
        )}

        <StyledForm onSubmit={onSubmit}>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
            <Typography variant="caption" fontWeight="500">{t('Login.emailLabel')}</Typography>
            <TextField
              variant="outlined"
              placeholder="user@example.com"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              disabled={isSubmitting}
              fullWidth
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <MailOutlineIcon color="disabled" />
                  </InputAdornment>
                ),
              }}
            />
          </Box>

          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
            <Typography variant="caption" fontWeight="500">{t('Login.passwordLabel')}</Typography>
            <TextField
              variant="outlined"
              placeholder="••••••••"
              type={showPassword ? 'text' : 'password'}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              disabled={isSubmitting}
              fullWidth
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <LockOutlinedIcon color="disabled" />
                  </InputAdornment>
                ),
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton onClick={() => setShowPassword(!showPassword)} edge="end" size="small">
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                )
              }}
            />
          </Box>

          <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
            <Link component={RouterLink} to="/forgot-password" variant="body2" color="primary">
              {t('Login.forgotPasswordLink')}
            </Link>
          </Box>

          <Button
            type="submit"
            variant="contained"
            color="primary"
            size="large"
            disabled={isSubmitting}
            fullWidth
            sx={{ height: 48 }}
          >
            {loading ? t('Common.loading') : t('Login.loginButton')}
          </Button>

          <DividerBox>
            <DividerText>Or</DividerText>
          </DividerBox>

          <SsoButton
            type="button"
            fullWidth
            disabled={isSubmitting}
            onClick={onGoogleLogin}
            startIcon={<GoogleIcon />}
          >
            {ssoLoading ? t('Common.loading') : 'Continue with Google'}
          </SsoButton>
        </StyledForm>

        <Box textAlign="center" mt={1}>
          <Typography variant="body2" color="text.secondary">
            {t('Login.registerLink').split('?')[0]}?{' '}
            <Link component={RouterLink} to="/register" color="primary" fontWeight="bold">
              Sign up
            </Link>
          </Typography>
        </Box>
      </LoginCard>
    </LoginContainer>
  );
};

export default LoginView;
