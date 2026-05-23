import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#555f71', // Charcoal
      dim: '#3e4652',
    },
    secondary: {
      main: '#5a6064', // Supporting info
    },
    tertiary: {
      main: '#00639e', // Essential Blue
    },
    error: {
      main: '#ffb4ab',
      container: '#93000a',
    },
    success: {
      main: '#81c784',
      container: '#0f5223', // Repurposed error_container for positive
    },
    background: {
      default: '#f4f6f8', // surface-dim
      paper: '#ffffff', // surface-container-lowest
    },
    surface: {
      main: '#f7f9fc',
      high: '#eef0f4',
      highest: '#e2e5ea',
    },
  },
  typography: {
    fontFamily: '"Inter", "Helvetica", "Arial", sans-serif',
    h1: {
      fontFamily: '"Manrope", sans-serif',
      fontSize: '3.5rem', // display-lg
      fontWeight: 700,
    },
    h2: {
      fontFamily: '"Manrope", sans-serif',
      fontSize: '2.5rem',
      fontWeight: 700,
    },
    h3: {
      fontFamily: '"Manrope", sans-serif',
      fontSize: '2rem',
      fontWeight: 700,
    },
    subtitle1: {
      fontSize: '1rem', // label-md
      fontWeight: 500,
    },
    body1: {
      fontSize: '1rem',
      fontWeight: 400,
    },
    caption: {
      fontSize: '0.75rem', // label-sm
    },
  },
  shape: {
    borderRadius: 16,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 16, // lg roundedness
          textTransform: 'none',
          boxShadow: 'none',
          '&:hover': {
            boxShadow: 'none',
          },
        },
        containedPrimary: {
          backgroundColor: '#555f71',
          color: '#ffffff',
        },
        containedSecondary: {
          backgroundColor: '#eef0f4', // surface-container-high
          color: '#1a1c1e', // on-surface
          border: 'none',
        },
        text: {
          textDecoration: 'underline',
          textDecorationColor: '#00639e',
          color: '#00639e', // tertiary
        }
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          '& .MuiOutlinedInput-root': {
            borderRadius: 24, // xl rounded corners
            backgroundColor: '#e2e5ea', // surface-container-highest
            '& fieldset': {
              border: 'none', // No default border
            },
            '&:hover fieldset': {
              border: 'none',
            },
            '&.Mui-focused fieldset': {
              border: '1px solid rgba(0, 99, 158, 0.4)', // Ghost Border fallback with tertiary 40%
            },
          },
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 24,
          boxShadow: '0px 24px 48px rgba(45, 52, 53, 0.06)', // Ambient Shadows
          border: 'none',
          backgroundColor: '#ffffff',
        },
      },
    },
  },
});

export default theme;
