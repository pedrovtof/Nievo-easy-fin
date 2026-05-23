import React, { useState, useEffect, useContext } from 'react';
import SettingsView from './View';
import { getSettingsData, updateSettings } from './api';
import { ColorModeContext } from '../../context/ThemeContext';

const Settings = () => {
  const { mode, toggleColorMode } = useContext(ColorModeContext);
  const [profile, setProfile] = useState(null);
  const [toggles, setToggles] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await getSettingsData();
        const responseData = response.data || response;
        if (responseData.profile && responseData.toggles) {
          setProfile(responseData.profile);
          setToggles(responseData.toggles);
        } else if (responseData.data) {
          setProfile(responseData.data.profile);
          setToggles(responseData.data.toggles);
        }
      } catch (err) {
        console.error(err);
        setError('Failed to load settings data');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleProfileChange = (key, value) => {
    setProfile(prev => ({ ...prev, [key]: value }));
  };

  const handleToggleChange = (key) => {
    setToggles(prev => ({ ...prev, [key]: !prev[key] }));
  };

  const handleSave = async () => {
    try {
      await updateSettings({ profile, toggles });
      alert('Settings saved successfully!');
    } catch (err) {
      alert('Failed to save settings.');
    }
  };

  const handleChangePassword = async () => {
    const oldPassword = prompt('Enter your current password:');
    if (!oldPassword) return;
    const newPassword = prompt('Enter your new password:');
    if (!newPassword) return;
    try {
      await import('./api').then(m => m.changePassword(profile.email, oldPassword, newPassword));
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
