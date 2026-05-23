import React, { useState } from 'react';
import MockGuidePopupView from './View';
import { mockResponses } from '../../services/mockData';

const MockGuidePopup = () => {
  const [open, setOpen] = useState(false);

  // Only render if in mock mode
  if (import.meta.env.VITE_USE_MOCK !== 'true') return null;

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const copyToClipboard = (text) => {
    navigator.clipboard.writeText(text);
  };

  const getMockDataString = () => JSON.stringify(mockResponses, null, 2);

  return (
    <MockGuidePopupView
      open={open}
      onOpen={handleOpen}
      onClose={handleClose}
      onCopy={copyToClipboard}
      mockDataString={getMockDataString()}
    />
  );
};

export default MockGuidePopup;
