import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { GoogleOAuthProvider } from '@react-oauth/google';
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeContextProvider } from './context/ThemeContext';
import './index.css';
import App from './App.jsx';
import MockGuidePopup from './components/MockGuidePopup';

const googleClientId = import.meta.env.VITE_GOOGLE_CLIENT_ID || 'dummy_client_id';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <GoogleOAuthProvider clientId={googleClientId}>
      <ThemeContextProvider>
        <CssBaseline />
        <App />
        <MockGuidePopup />
      </ThemeContextProvider>
    </GoogleOAuthProvider>
  </StrictMode>,
);
