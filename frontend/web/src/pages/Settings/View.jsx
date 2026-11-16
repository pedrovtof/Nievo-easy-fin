import React from 'react';
import {
  Typography,
  Box,
  CircularProgress,
  TextField,
  MenuItem,
  Switch,
  Button,
  Grid,
  FormControlLabel,
  Chip,
  Alert,
} from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import LockIcon from '@mui/icons-material/Lock';
import TuneIcon from '@mui/icons-material/Tune';
import SaveIcon from '@mui/icons-material/Save';
import VerifiedUserIcon from '@mui/icons-material/VerifiedUser';
import { useText } from '../../hooks/useText';
import {
  SettingsContainer,
  ProfileHeroCard,
  SectionCard,
  SectionHeader,
  IconBadge,
  DetailFieldBox,
} from './styles';

/**
 * Formats an integer or string phone number into a human-friendly representation.
 *
 * @param {number|string|null|undefined} phone - Raw phone value from User/details.
 * @returns {string} Formatted phone string or fallback message.
 */
export const formatUserPhone = (phone) => {
  if (phone === null || phone === undefined || String(phone).trim() === '') {
    return 'Not registered';
  }
  return String(phone);
};

/**
 * Settings View Template
 * Pure presentation component for displaying authenticated user details (name, email, phone, status)
 * and editing preferences under The Silent Navigator design system.
 *
 * @param {Object} props - Properties mapping state and handlers from the Controller.
 * @returns {JSX.Element} The rendered visual layer of the Settings page.
 */
const SettingsView = ({
  profile,
  toggles,
  loading,
  error,
  globalDarkMode,
  onGlobalDarkModeToggle,
  onProfileChange,
  onToggleChange,
  onSave,
  onChangePassword,
}) => {
  const { t } = useText();

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" p={6}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }

  if (!profile || !toggles) return null;

  const isStatusActive = String(profile.status || '').toUpperCase() === 'ACTIVE';

  return (
    <SettingsContainer>
      <Box maxWidth={650}>
        <Typography variant="h2" color="primary">
          {t('Settings.title')}
        </Typography>
        <Typography variant="body1" color="secondary" mt={0.5}>
          Review your verified account details, security credentials, and personal preferences.
        </Typography>
      </Box>

      <ProfileHeroCard>
        <Box display="flex" flexDirection="column" gap={0.5}>
          <Typography
            variant="caption"
            sx={{ opacity: 0.85, textTransform: 'uppercase', letterSpacing: 1 }}
          >
            Authenticated Account Holder
          </Typography>
          <Typography variant="h2" sx={{ color: '#ffffff' }}>
            {profile.fullName || 'Nievo User'}
          </Typography>
          <Typography variant="body2" sx={{ opacity: 0.9 }}>
            {profile.email}
          </Typography>
        </Box>
        <Box display="flex" alignItems="center" gap={1.5}>
          <Chip
            icon={<VerifiedUserIcon style={{ color: '#ffffff' }} fontSize="small" />}
            label={profile.status || 'ACTIVE'}
            sx={{
              bgcolor: isStatusActive ? 'rgba(200, 246, 197, 0.22)' : 'rgba(255, 218, 214, 0.22)',
              color: '#ffffff',
              fontWeight: 700,
              letterSpacing: 0.5,
              textTransform: 'uppercase',
            }}
          />
        </Box>
      </ProfileHeroCard>

      <Grid container spacing={4}>
        <Grid item xs={12} md={7}>
          <Box display="flex" flexDirection="column" gap={4}>
            <SectionCard>
              <SectionHeader>
                <IconBadge>
                  <PersonIcon />
                </IconBadge>
                <Box>
                  <Typography variant="h6" color="primary" fontWeight="bold">
                    User Details
                  </Typography>
                  <Typography variant="caption" color="secondary">
                    Synchronized from your Nievo EasyFin profile
                  </Typography>
                </Box>
              </SectionHeader>

              <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                  <DetailFieldBox>
                    <Typography variant="caption" color="secondary" sx={{ textTransform: 'uppercase', letterSpacing: 0.5 }}>
                      Account Status
                    </Typography>
                    <Typography variant="subtitle1" color="primary" fontWeight={700}>
                      {profile.status || 'ACTIVE'}
                    </Typography>
                  </DetailFieldBox>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <DetailFieldBox>
                    <Typography variant="caption" color="secondary" sx={{ textTransform: 'uppercase', letterSpacing: 0.5 }}>
                      Registered Phone
                    </Typography>
                    <Typography variant="subtitle1" color="primary" fontWeight={700}>
                      {formatUserPhone(profile.phone)}
                    </Typography>
                  </DetailFieldBox>
                </Grid>
              </Grid>

              <Box display="flex" flexDirection="column" gap={3}>
                <Grid container spacing={2}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Full Name"
                      value={profile.fullName}
                      onChange={(e) => onProfileChange('fullName', e.target.value)}
                      fullWidth
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Email Address"
                      type="email"
                      value={profile.email}
                      onChange={(e) => onProfileChange('email', e.target.value)}
                      fullWidth
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Phone Number"
                      value={profile.phone ?? ''}
                      onChange={(e) => onProfileChange('phone', e.target.value)}
                      placeholder="Not registered"
                      fullWidth
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Status"
                      value={profile.status || 'ACTIVE'}
                      InputProps={{ readOnly: true }}
                      fullWidth
                    />
                  </Grid>
                </Grid>
                <TextField
                  select
                  label="Default Currency"
                  value={profile.currency}
                  onChange={(e) => onProfileChange('currency', e.target.value)}
                  fullWidth
                  helperText="This will be the default currency for your budget reports."
                >
                  <MenuItem value="USD">USD - United States Dollar</MenuItem>
                  <MenuItem value="BRL">BRL - Brazilian Real</MenuItem>
                  <MenuItem value="EUR">EUR - Euro</MenuItem>
                  <MenuItem value="GBP">GBP - British Pound</MenuItem>
                  <MenuItem value="CAD">CAD - Canadian Dollar</MenuItem>
                </TextField>
              </Box>
            </SectionCard>

            <SectionCard>
              <SectionHeader>
                <IconBadge>
                  <LockIcon />
                </IconBadge>
                <Box>
                  <Typography variant="h6" color="primary" fontWeight="bold">
                    Security
                  </Typography>
                  <Typography variant="caption" color="secondary">
                    Manage authentication and account protection
                  </Typography>
                </Box>
              </SectionHeader>
              <Box display="flex" justifyContent="space-between" alignItems="center">
                <Box>
                  <Typography variant="body2" fontWeight="bold">
                    Password
                  </Typography>
                  <Typography variant="caption" color="text.secondary">
                    Protected with PBKDF2 cryptographic hashing
                  </Typography>
                </Box>
                <Button variant="outlined" onClick={onChangePassword}>
                  Change Password
                </Button>
              </Box>
              <Box display="flex" justifyContent="space-between" alignItems="center">
                <Box>
                  <Typography variant="body2" fontWeight="bold">
                    Two-Factor Authentication (2FA)
                  </Typography>
                  <Typography variant="caption" color="text.secondary">
                    Add an extra layer of security.
                  </Typography>
                </Box>
                <Switch checked={toggles.twoFactor} onChange={() => onToggleChange('twoFactor')} />
              </Box>
            </SectionCard>
          </Box>
        </Grid>

        <Grid item xs={12} md={5}>
          <Box display="flex" flexDirection="column" gap={4}>
            <SectionCard>
              <SectionHeader>
                <IconBadge>
                  <TuneIcon />
                </IconBadge>
                <Box>
                  <Typography variant="h6" color="primary" fontWeight="bold">
                    Preferences
                  </Typography>
                  <Typography variant="caption" color="secondary">
                    Customize display theme and notification alerts
                  </Typography>
                </Box>
              </SectionHeader>

              <Box display="flex" justifyContent="space-between" alignItems="center">
                <Typography variant="body2" fontWeight="bold">
                  Dark Mode
                </Typography>
                <Switch checked={globalDarkMode} onChange={onGlobalDarkModeToggle} />
              </Box>

              <Box mt={1} display="flex" flexDirection="column" gap={1}>
                <Typography variant="body2" fontWeight="bold" mb={1}>
                  Notifications
                </Typography>
                <FormControlLabel
                  control={
                    <Switch
                      checked={toggles.weeklyDigest}
                      onChange={() => onToggleChange('weeklyDigest')}
                    />
                  }
                  label={<Typography variant="body2">Weekly Spending Digest</Typography>}
                />
                <FormControlLabel
                  control={
                    <Switch
                      checked={toggles.unusualActivity}
                      onChange={() => onToggleChange('unusualActivity')}
                    />
                  }
                  label={<Typography variant="body2">Unusual Activity Alerts</Typography>}
                />
                <FormControlLabel
                  control={
                    <Switch
                      checked={toggles.budgetWarnings}
                      onChange={() => onToggleChange('budgetWarnings')}
                    />
                  }
                  label={<Typography variant="body2">Budget Limit Warnings</Typography>}
                />
              </Box>
            </SectionCard>

            <SectionCard>
              <Button
                variant="contained"
                color="primary"
                startIcon={<SaveIcon />}
                fullWidth
                size="large"
                onClick={onSave}
                sx={{ mb: 1 }}
              >
                Save Changes
              </Button>
              <Button variant="text" color="inherit" fullWidth size="large">
                Cancel
              </Button>
            </SectionCard>
          </Box>
        </Grid>
      </Grid>
    </SettingsContainer>
  );
};

export default SettingsView;
