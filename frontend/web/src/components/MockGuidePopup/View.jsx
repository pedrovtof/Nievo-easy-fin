import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Fab, Typography, Box, TextField, IconButton, Chip } from '@mui/material';
import DataObjectIcon from '@mui/icons-material/DataObject';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import { FabContainer } from './styles';

const MockField = ({ label, value, onCopy }) => (
  <Box display="flex" alignItems="center" gap={1} mb={2}>
    <TextField
      label={label}
      value={value || ''}
      fullWidth
      variant="outlined"
      size="small"
      InputProps={{
        readOnly: true,
      }}
    />
    <IconButton onClick={() => onCopy(value)} title="Copy value">
      <ContentCopyIcon />
    </IconButton>
  </Box>
);

/**
 * Mock Guide Popup View
 * Presentation layer for the mock data overlay.
 *
 * @param {Object} props - Properties mapping state and handlers from the Controller.
 * @returns {JSX.Element} The rendered dialog.
 */
const MockGuidePopupView = ({ open, mockData, onOpen, onClose, onCopy }) => {
  return (
    <>
      <FabContainer>
        <Fab color="tertiary" aria-label="mock data" onClick={onOpen}>
          <DataObjectIcon style={{ color: '#fff' }} />
        </Fab>
      </FabContainer>

      <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
        <DialogTitle>Mock Data Guide</DialogTitle>
        <DialogContent>
          <Typography variant="body2" gutterBottom mb={3}>
            Here is the current state of the in-memory mock database.
            All Core API endpoints (Payments, Banks, Cards, Goals, and Categories) are intercepted in mock mode.
          </Typography>
          
          {mockData?.users?.map((user, idx) => (
            <Box key={idx} mb={3} p={2.5} bgcolor="surface.main" borderRadius={2}>
              <Typography variant="subtitle2" color="primary" gutterBottom>
                {idx === 0 ? "Default User" : `Created User ${idx}`}
              </Typography>
              <MockField label="Email" value={user.email} onCopy={onCopy} />
              <MockField label="Password" value={user.password} onCopy={onCopy} />
            </Box>
          ))}

          {mockData?.counts && (
            <>
              <Typography variant="subtitle2" color="primary" gutterBottom mt={2}>
                Active Core Mock Entities
              </Typography>
              <Box display="flex" flexWrap="wrap" gap={1} mb={3} p={2} bgcolor="surface.main" borderRadius={2}>
                <Chip label={`Banks: ${mockData.counts.userBanks}`} size="small" />
                <Chip label={`Cards: ${mockData.counts.userCards}`} size="small" />
                <Chip label={`Goals: ${mockData.counts.goals}`} size="small" />
                <Chip label={`Categories: ${mockData.counts.categories}`} size="small" />
                <Chip label={`Transactions: ${mockData.counts.transactions}`} size="small" />
              </Box>
            </>
          )}

          <Typography variant="subtitle2" color="primary" gutterBottom mt={2}>
            Dashboard Data
          </Typography>
          <Box p={2.5} bgcolor="surface.main" borderRadius={2}>
            <MockField label="Total Balance" value={mockData?.dashboard?.totalBalance} onCopy={onCopy} />
            <MockField label="Income" value={mockData?.dashboard?.income} onCopy={onCopy} />
            <MockField label="Expenses" value={mockData?.dashboard?.expenses} onCopy={onCopy} />
          </Box>

        </DialogContent>
        <DialogActions sx={{ p: 2.5 }}>
          <Button onClick={onClose} color="primary" variant="contained">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default MockGuidePopupView;
