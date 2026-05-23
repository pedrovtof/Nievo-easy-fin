import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Fab, Typography, Box } from '@mui/material';
import DataObjectIcon from '@mui/icons-material/DataObject';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import { FabContainer, PreformattedText } from './styles';

const MockGuidePopupView = ({ open, onOpen, onClose, onCopy, mockDataString }) => {
  return (
    <>
      <FabContainer>
        <Fab color="tertiary" aria-label="mock data" onClick={onOpen}>
          <DataObjectIcon style={{ color: '#fff' }} />
        </Fab>
      </FabContainer>

      <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
        <DialogTitle>Mock Data Guide</DialogTitle>
        <DialogContent dividers>
          <Typography variant="body1" gutterBottom>
            You are currently running the app in Mock Mode. API calls are intercepted and return the following data.
          </Typography>
          <Box position="relative">
            <PreformattedText>{mockDataString}</PreformattedText>
            <Button
              variant="contained"
              size="small"
              startIcon={<ContentCopyIcon />}
              onClick={() => onCopy(mockDataString)}
              style={{ position: 'absolute', top: 16, right: 16 }}
            >
              Copy Data
            </Button>
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default MockGuidePopupView;
