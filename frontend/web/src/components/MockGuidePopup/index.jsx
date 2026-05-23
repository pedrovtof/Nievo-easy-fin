import React, { useState } from 'react';
import MockGuidePopupView from './View';
import { getMockState, updateMockState } from '../../services/mockData';

const MockGuidePopup = () => {
  const [open, setOpen] = useState(false);
  const [formData, setFormData] = useState({});

  // Only render if in mock mode
  if (import.meta.env.VITE_USE_MOCK !== 'true') return null;

  const handleOpen = () => {
    setFormData(getMockState());
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  const copyToClipboard = (text) => {
    navigator.clipboard.writeText(text);
  };

  const handleFieldChange = (field, value) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  const handleSubmit = () => {
    updateMockState(formData);
    alert('Mock data updated for this session!');
    handleClose();
  };

  return (
    <MockGuidePopupView
      open={open}
      formData={formData}
      onOpen={handleOpen}
      onClose={handleClose}
      onCopy={copyToClipboard}
      onChange={handleFieldChange}
      onSubmit={handleSubmit}
    />
  );
};

export default MockGuidePopup;
