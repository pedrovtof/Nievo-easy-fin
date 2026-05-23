import React, { useState } from 'react';
import MockGuidePopupView from './View';
import { getMockState } from '../../services/mockData';

const MockGuidePopup = () => {
  const [open, setOpen] = useState(false);
  const [mockData, setMockData] = useState(null);

  const handleOpen = () => {
    setMockData(getMockState());
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  const copyToClipboard = (text) => {
    navigator.clipboard.writeText(text);
  };

  // Only render if in mock mode
  if (import.meta.env.VITE_USE_MOCK !== 'true') return null;

  return (
    <MockGuidePopupView
      open={open}
      mockData={mockData}
      onOpen={handleOpen}
      onClose={handleClose}
      onCopy={copyToClipboard}
    />
  );
};

export default MockGuidePopup;
