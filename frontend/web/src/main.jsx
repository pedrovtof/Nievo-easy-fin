import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { GoogleOAuthProvider } from '@react-oauth/google';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import theme from './theme/theme';
import './index.css';
import App from './App.jsx';
import MockGuidePopup from './components/MockGuidePopup';

const googleClientId = import.meta.env.VITE_GOOGLE_CLIENT_ID || 'dummy_client_id';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <GoogleOAuthProvider clientId={googleClientId}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <App />
        <MockGuidePopup />
      </ThemeProvider>
    </GoogleOAuthProvider>
  </StrictMode>,
);
