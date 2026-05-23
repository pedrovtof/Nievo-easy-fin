import React, { useState } from 'react';
import MockGuidePopupView from './View';
import { getMockState } from '../../services/mockData';

/**
 * Mock Guide Popup Controller
 * Manages the floating action button and dialog state to expose in-memory mock data.
 * Useful for development to copy generated credentials.
 *
 * @returns {JSX.Element|null} The rendered popup or null if not in mock mode.
 */
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
