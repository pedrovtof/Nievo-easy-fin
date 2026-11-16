import React, { useState, useEffect, useContext, useCallback } from 'react';
import SettingsView from './View';
import { getUserDetails, getSettingsData, updateSettings, changePassword } from './api';
import { ThemeContext } from '../../context/ThemeContext';

/**
 * Normalizes raw user details returned by GET /api/public/v1/User/details into the Settings profile shape.
 *
 * @param {Object} raw - Raw user details object ({ email, name, phone, status }).
 * @returns {Object} Normalized profile object for Settings state.
 */
export const normalizeUserDetailsProfile = (raw = {}) => ({
  fullName: raw.name || raw.fullName || '',
  email: raw.email || '',
  phone: raw.phone !== null && raw.phone !== undefined ? String(raw.phone) : '',
  status: raw.status || 'ACTIVE',
  currency: raw.currency || 'USD',
});

/**
 * Settings Page Controller
 * Manages state and logic for displaying authenticated user details (GET /api/public/v1/User/details)
 * and updating user preferences.
 *
 * @returns {JSX.Element} The rendered Settings page controller.
 */
const Settings = () => {
  const { mode, toggleColorMode } = useContext(ThemeContext);
  const [profile, setProfile] = useState(null);
  const [toggles, setToggles] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchData = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const [detailsResponse, settingsResponse] = await Promise.all([
        getUserDetails(),
        getSettingsData(),
      ]);

      const detailsPayload = detailsResponse?.data?.data ?? detailsResponse?.data ?? {};
      const settingsPayload = settingsResponse?.data ?? settingsResponse ?? {};

      setProfile(
        normalizeUserDetailsProfile({
          ...settingsPayload.profile,
          ...detailsPayload,
        })
      );
      setToggles(
        settingsPayload.toggles || {
          twoFactor: false,
          darkMode: false,
          weeklyDigest: true,
          unusualActivity: true,
          budgetWarnings: false,
        }
      );
    } catch {
      setError('Failed to load user details and settings data');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleProfileChange = (key, value) => {
    setProfile((prev) => ({ ...prev, [key]: value }));
  };

  const handleToggleChange = (key) => {
    setToggles((prev) => ({ ...prev, [key]: !prev[key] }));
  };

  const handleSave = async () => {
    try {
      await updateSettings({ profile, toggles });
      alert('Settings saved successfully!');
    } catch {
      alert('Failed to save settings.');
    }
  };

  const handleChangePassword = async () => {
    const oldPassword = prompt('Enter your current password:');
    if (!oldPassword) return;
    const newPassword = prompt('Enter your new password:');
    if (!newPassword) return;
    try {
      await changePassword(profile.email, oldPassword, newPassword);
      alert('Password updated successfully!');
    } catch (err) {
      alert(err.response?.data?.messages?.[0] || 'Failed to update password. Check your current password.');
    }
  };

  return (
    <SettingsView
      profile={profile}
      toggles={toggles}
      loading={loading}
      error={error}
      globalDarkMode={mode === 'dark'}
      onGlobalDarkModeToggle={toggleColorMode}
      onProfileChange={handleProfileChange}
      onToggleChange={handleToggleChange}
      onSave={handleSave}
      onChangePassword={handleChangePassword}
    />
  );
};

export default Settings;
