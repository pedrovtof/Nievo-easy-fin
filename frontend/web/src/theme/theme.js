const getDesignTokens = (mode) => ({
  palette: {
    mode,
    ...(mode === 'light'
      ? {
          primary: { main: '#555f71', dim: '#3e4652' },
          secondary: { main: '#5a6064' },
          tertiary: { main: '#00639e' },
          error: { main: '#ba1a1a', container: '#ffdad6' },
          success: { main: '#146c2e', container: '#c8f6c5' },
          background: { default: '#f4f6f8', paper: '#ffffff' },
          surface: { main: '#f7f9fc', high: '#eef0f4', highest: '#e2e5ea' },
          text: { primary: '#1a1c1e', secondary: '#43474e' },
        }
      : {
          primary: { main: '#bec6dc', dim: '#a3ad2' },
          secondary: { main: '#c2c7ce' },
          tertiary: { main: '#86cbff' },
          error: { main: '#ffb4ab', container: '#93000a' },
          success: { main: '#81c784', container: '#0f5223' },
          background: { default: '#1a1c1e', paper: '#232527' },
          surface: { main: '#1e2022', high: '#2a2c2e', highest: '#35373a' },
          text: { primary: '#e3e2e6', secondary: '#c3c6cf' },
        }),
  },
  typography: {
    fontFamily: '"Inter", "Helvetica", "Arial", sans-serif',
    h1: { fontFamily: '"Manrope", sans-serif', fontSize: '3.5rem', fontWeight: 700 },
    h2: { fontFamily: '"Manrope", sans-serif', fontSize: '2.5rem', fontWeight: 700 },
    h3: { fontFamily: '"Manrope", sans-serif', fontSize: '2rem', fontWeight: 700 },
    subtitle1: { fontSize: '1rem', fontWeight: 500 },
    body1: { fontSize: '1rem', fontWeight: 400 },
    caption: { fontSize: '0.75rem' },
  },
  shape: { borderRadius: 16 },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 16,
          textTransform: 'none',
          boxShadow: 'none',
          '&:hover': { boxShadow: 'none' },
        },
        containedSecondary: ({ theme }) => ({
          backgroundColor: theme.palette.surface.high,
          color: theme.palette.text.primary,
          border: 'none',
        }),
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: ({ theme }) => ({
          '& .MuiOutlinedInput-root': {
            borderRadius: 24,
            backgroundColor: theme.palette.surface.highest,
            '& fieldset': { border: 'none' },
            '&:hover fieldset': { border: 'none' },
            '&.Mui-focused fieldset': { border: `1px solid ${theme.palette.tertiary.main}` },
          },
        }),
      },
    },
    MuiCard: {
      styleOverrides: {
        root: ({ theme }) => ({
          borderRadius: 24,
          boxShadow: mode === 'light' ? '0px 24px 48px rgba(45, 52, 53, 0.06)' : 'none',
          border: mode === 'light' ? 'none' : `1px solid ${theme.palette.divider}`,
          backgroundColor: theme.palette.background.paper,
        }),
      },
    },
  },
});

export { getDesignTokens };
