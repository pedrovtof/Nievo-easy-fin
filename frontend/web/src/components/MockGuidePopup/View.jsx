import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Fab, Typography, Box, TextField, IconButton } from '@mui/material';
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
        <DialogContent dividers>
          <Typography variant="body2" gutterBottom mb={3}>
            Here is the current state of the mock database. 
            Any users created during this session will appear below, making it easy to copy their credentials.
          </Typography>
          
          {mockData?.users?.map((user, idx) => (
            <Box key={idx} mb={3} p={2} border="1px solid" borderColor="divider" borderRadius={1}>
              <Typography variant="subtitle2" color="primary" gutterBottom>
                {idx === 0 ? "Default User" : `Created User ${idx}`}
              </Typography>
              <MockField label="Email" value={user.email} onCopy={onCopy} />
              <MockField label="Password" value={user.password} onCopy={onCopy} />
            </Box>
          ))}

          <Typography variant="subtitle2" color="primary" gutterBottom mt={3}>
            Dashboard Data
          </Typography>
          <Box p={2} border="1px solid" borderColor="divider" borderRadius={1}>
            <MockField label="Total Balance" value={mockData?.dashboard?.totalBalance} onCopy={onCopy} />
            <MockField label="Income" value={mockData?.dashboard?.income} onCopy={onCopy} />
            <MockField label="Expenses" value={mockData?.dashboard?.expenses} onCopy={onCopy} />
          </Box>

        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} color="primary" variant="contained">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default MockGuidePopupView;
