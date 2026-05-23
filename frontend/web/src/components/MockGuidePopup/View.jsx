import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Fab, Typography, Box, TextField, IconButton } from '@mui/material';
import DataObjectIcon from '@mui/icons-material/DataObject';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import SaveIcon from '@mui/icons-material/Save';
import { FabContainer } from './styles';

const MockField = ({ label, value, onChange, onCopy }) => (
  <Box display="flex" alignItems="center" gap={1} mb={2}>
    <TextField
      label={label}
      value={value || ''}
      onChange={onChange}
      fullWidth
      variant="outlined"
      size="small"
    />
    <IconButton onClick={() => onCopy(value)} title="Copy value">
      <ContentCopyIcon />
    </IconButton>
  </Box>
);

const MockGuidePopupView = ({ open, formData, onOpen, onClose, onCopy, onChange, onSubmit }) => {
  return (
    <>
      <FabContainer>
        <Fab color="tertiary" aria-label="mock data" onClick={onOpen}>
          <DataObjectIcon style={{ color: '#fff' }} />
        </Fab>
      </FabContainer>

      <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
        <DialogTitle>Mock Data Editor</DialogTitle>
        <DialogContent dividers>
          <Typography variant="body2" gutterBottom mb={3}>
            Edit the temporary mock data stored in memory. Changes will persist until the server restarts.
          </Typography>
          
          <Typography variant="subtitle2" color="primary" gutterBottom>
            Default User Credentials
          </Typography>
          <MockField label="Email" value={formData.userEmail} onChange={(e) => onChange('userEmail', e.target.value)} onCopy={onCopy} />
          <MockField label="Password" value={formData.userPassword} onChange={(e) => onChange('userPassword', e.target.value)} onCopy={onCopy} />

          <Typography variant="subtitle2" color="primary" gutterBottom mt={3}>
            Dashboard Data
          </Typography>
          <MockField label="Total Balance" value={formData.totalBalance} onChange={(e) => onChange('totalBalance', e.target.value)} onCopy={onCopy} />
          <MockField label="Income" value={formData.income} onChange={(e) => onChange('income', e.target.value)} onCopy={onCopy} />
          <MockField label="Expenses" value={formData.expenses} onChange={(e) => onChange('expenses', e.target.value)} onCopy={onCopy} />

        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} color="inherit">
            Cancel
          </Button>
          <Button onClick={onSubmit} color="primary" variant="contained" startIcon={<SaveIcon />}>
            Submit Changes
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default MockGuidePopupView;
