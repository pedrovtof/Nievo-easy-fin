import React from 'react';
import { Typography, Box, CircularProgress, TextField, MenuItem, Switch, Button, Grid, FormControlLabel } from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import LockIcon from '@mui/icons-material/Lock';
import TuneIcon from '@mui/icons-material/Tune';
import SaveIcon from '@mui/icons-material/Save';
import { useText } from '../../hooks/useText';
import { SettingsContainer, SectionCard, SectionHeader } from './styles';

/**
 * Settings View Template
 * Pure presentation component for editing the user profile and preferences.
 *
 * @param {Object} props - Properties mapping state and handlers from the Controller.
 * @returns {JSX.Element} The rendered visual layer of the Settings page.
 */
const SettingsView = ({ profile, toggles, loading, error, globalDarkMode, onGlobalDarkModeToggle, onProfileChange, onToggleChange, onSave, onChangePassword }) => {
  const { t } = useText();

  if (loading) {
    return <Box display="flex" justifyContent="center" p={4}><CircularProgress /></Box>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  if (!profile || !toggles) return null;

  return (
    <SettingsContainer>
      <Box mb={2}>
        <Typography variant="h2" color="primary">{t('Settings.title')}</Typography>
        <Typography variant="body2" color="text.secondary">Manage your profile, security, and application settings.</Typography>
      </Box>

      <Grid container spacing={4}>
        <Grid item xs={12} md={7}>
          <Box display="flex" flexDirection="column" gap={4}>
            <SectionCard>
              <SectionHeader>
                <PersonIcon color="primary" />
                <Typography variant="h6" color="primary" fontWeight="bold">Profile Information</Typography>
              </SectionHeader>
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
                  <MenuItem value="EUR">EUR - Euro</MenuItem>
                  <MenuItem value="GBP">GBP - British Pound</MenuItem>
                  <MenuItem value="CAD">CAD - Canadian Dollar</MenuItem>
                </TextField>
              </Box>
            </SectionCard>

            <SectionCard>
              <SectionHeader>
                <LockIcon color="primary" />
                <Typography variant="h6" color="primary" fontWeight="bold">Security</Typography>
              </SectionHeader>
              <Box display="flex" justifyContent="space-between" alignItems="center">
                <Box>
                  <Typography variant="body2" fontWeight="bold">Password</Typography>
                  <Typography variant="caption" color="text.secondary">Last changed 3 months ago</Typography>
                </Box>
                <Button variant="outlined" onClick={onChangePassword}>Change Password</Button>
              </Box>
              <Box display="flex" justifyContent="space-between" alignItems="center">
                <Box>
                  <Typography variant="body2" fontWeight="bold">Two-Factor Authentication (2FA)</Typography>
                  <Typography variant="caption" color="text.secondary">Add an extra layer of security.</Typography>
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
                <TuneIcon color="primary" />
                <Typography variant="h6" color="primary" fontWeight="bold">Preferences</Typography>
              </SectionHeader>
              
              <Box display="flex" justifyContent="space-between" alignItems="center">
                <Typography variant="body2" fontWeight="bold">Dark Mode</Typography>
                <Switch checked={globalDarkMode} onChange={onGlobalDarkModeToggle} />
              </Box>

              <Box mt={2}>
                <Typography variant="body2" fontWeight="bold" mb={2}>Notifications</Typography>
                <FormControlLabel
                  control={<Switch checked={toggles.weeklyDigest} onChange={() => onToggleChange('weeklyDigest')} />}
                  label={<Typography variant="body2">Weekly Spending Digest</Typography>}
                />
                <FormControlLabel
                  control={<Switch checked={toggles.unusualActivity} onChange={() => onToggleChange('unusualActivity')} />}
                  label={<Typography variant="body2">Unusual Activity Alerts</Typography>}
                />
                <FormControlLabel
                  control={<Switch checked={toggles.budgetWarnings} onChange={() => onToggleChange('budgetWarnings')} />}
                  label={<Typography variant="body2">Budget Limit Warnings</Typography>}
                />
              </Box>
            </SectionCard>

            <SectionCard>
              <Button variant="contained" color="primary" startIcon={<SaveIcon />} fullWidth size="large" onClick={onSave} sx={{ mb: 2 }}>
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
