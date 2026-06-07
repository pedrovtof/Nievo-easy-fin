import React, { useContext } from 'react';
import {
  Box, Typography, CircularProgress, Alert, Chip, Divider, IconButton, Tooltip,
} from '@mui/material';
import { styled, useTheme } from '@mui/material/styles';
import GavelIcon from '@mui/icons-material/Gavel';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import LightModeIcon from '@mui/icons-material/LightMode';
import { ThemeContext } from '../../context/ThemeContext';

/* ── Styled components ──────────────────────────────────────────────────── */

const PageRoot = styled(Box)(({ theme }) => ({
  minHeight: '100vh',
  backgroundColor: theme.palette.background.default,
  display: 'flex',
  flexDirection: 'column',
}));

const TopBar = styled(Box)(({ theme }) => ({
  position: 'sticky',
  top: 0,
  zIndex: 100,
  width: '100%',
  padding: theme.spacing(2, 4),
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  backdropFilter: 'blur(20px)',
  WebkitBackdropFilter: 'blur(20px)',
  backgroundColor:
    theme.palette.mode === 'dark'
      ? 'rgba(26, 28, 30, 0.85)'
      : 'rgba(244, 246, 248, 0.85)',
  borderBottom: `1px solid ${theme.palette.divider}`,
}));

const ContentWrapper = styled(Box)(({ theme }) => ({
  flex: 1,
  display: 'flex',
  justifyContent: 'center',
  padding: theme.spacing(6, 2, 10),
}));

const ContentCard = styled(Box)(({ theme }) => ({
  width: '100%',
  maxWidth: 820,
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(5),
}));

const HeroSection = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(2),
}));

const IconBadge = styled(Box)(({ theme }) => ({
  display: 'inline-flex',
  alignItems: 'center',
  justifyContent: 'center',
  width: 64,
  height: 64,
  borderRadius: 20,
  background:
    theme.palette.mode === 'dark'
      ? 'linear-gradient(135deg, rgba(190,198,220,0.15) 0%, rgba(190,198,220,0.05) 100%)'
      : 'linear-gradient(135deg, rgba(85,95,113,0.12) 0%, rgba(85,95,113,0.04) 100%)',
  border: `1px solid ${
    theme.palette.mode === 'dark'
      ? 'rgba(190,198,220,0.2)'
      : 'rgba(85,95,113,0.15)'
  }`,
  marginBottom: theme.spacing(1),
}));

/* ── Prose area — renders API HTML safely inside an isolated container ─── */
const ProseContainer = styled(Box)(({ theme }) => ({
  backgroundColor: theme.palette.background.paper,
  borderRadius: 24,
  padding: theme.spacing(5, 6),
  border: `1px solid ${theme.palette.divider}`,
  boxShadow:
    theme.palette.mode === 'light'
      ? '0px 24px 48px rgba(45, 52, 53, 0.06)'
      : 'none',

  /* Typography rhythm for raw HTML content from the API */
  '& *': { fontFamily: '"Inter", "Helvetica", sans-serif !important', boxSizing: 'border-box' },
  '& h1, & h2, & h3, & h4': {
    fontFamily: '"Manrope", sans-serif !important',
    color: theme.palette.text.primary,
    marginTop: '2em',
    marginBottom: '0.5em',
    lineHeight: 1.3,
    fontWeight: 700,
  },
  '& h1': { fontSize: '1.75rem', borderBottom: `2px solid ${theme.palette.primary.main}`, paddingBottom: '0.4em' },
  '& h2': { fontSize: '1.3rem', color: theme.palette.primary.main },
  '& h3': { fontSize: '1.1rem' },
  '& p': {
    color: theme.palette.text.secondary,
    lineHeight: 1.8,
    margin: '0.6em 0',
    fontSize: '0.97rem',
  },
  '& strong, & b': { color: theme.palette.text.primary, fontWeight: 600 },
  '& a': { color: theme.palette.tertiary.main, textDecoration: 'underline' },
  '& ul, & ol': { paddingLeft: '1.5em', color: theme.palette.text.secondary, lineHeight: 1.8 },
  '& li': { marginBottom: '0.3em', fontSize: '0.97rem' },
  '& blockquote': {
    borderLeft: `3px solid ${theme.palette.primary.main}`,
    margin: '1em 0',
    paddingLeft: '1em',
    color: theme.palette.text.secondary,
    fontStyle: 'italic',
  },
  '& hr': { border: 'none', borderTop: `1px solid ${theme.palette.divider}`, margin: '2em 0' },
  '& table': {
    width: '100%',
    borderCollapse: 'collapse',
    margin: '1.5em 0',
    fontSize: '0.9rem',
    overflowX: 'auto',
    display: 'block',
  },
  '& th': {
    backgroundColor: theme.palette.surface.high,
    color: theme.palette.text.primary,
    fontWeight: 700,
    padding: '10px 14px',
    textAlign: 'left',
    borderBottom: `2px solid ${theme.palette.primary.main}`,
    whiteSpace: 'nowrap',
  },
  '& td': {
    padding: '9px 14px',
    color: theme.palette.text.secondary,
    borderBottom: `1px solid ${theme.palette.divider}`,
    verticalAlign: 'top',
  },
  '& tr:last-child td': { borderBottom: 'none' },
  '& tr:hover td': {
    backgroundColor:
      theme.palette.mode === 'dark'
        ? 'rgba(255,255,255,0.03)'
        : 'rgba(0,0,0,0.02)',
  },
  '& footer': {
    marginTop: '3rem',
    paddingTop: '1.5rem',
    borderTop: `1px solid ${theme.palette.divider}`,
    color: theme.palette.text.secondary,
    fontSize: '0.85rem',
    textAlign: 'center',
  },

  /* Hide any <head>, <html>, <body>, <style> tags that might come from raw HTML */
  '& head, & style': { display: 'none' },

  [theme.breakpoints.down('sm')]: {
    padding: theme.spacing(3, 2.5),
  },
}));

/* ══════════════════════════════════════════════════════════════════════════
   TermsView
   Props:
     content    {string}   Raw HTML string from the API
     version    {number}   Terms version number
     isLoading  {boolean}
     error      {string}
══════════════════════════════════════════════════════════════════════════ */
const TermsView = ({ content, version, isLoading, error }) => {
  const theme = useTheme();
  const { mode, toggleColorMode } = useContext(ThemeContext);

  return (
    <PageRoot>
      {/* ── Sticky top bar ─────────────────────────────────────────────── */}
      <TopBar>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
          <GavelIcon sx={{ color: 'primary.main', fontSize: 22 }} />
          <Typography
            variant="h3"
            sx={{ fontSize: '1.1rem', color: 'primary.main', fontWeight: 700 }}
          >
            Nievo Easy Fin
          </Typography>
          <Divider orientation="vertical" flexItem sx={{ mx: 0.5 }} />
          <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
            Termos de Uso
          </Typography>
        </Box>

        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          {version != null && (
            <Chip
              label={`v${version}`}
              size="small"
              sx={{
                fontWeight: 600,
                fontSize: '0.72rem',
                backgroundColor: 'surface.high',
                color: 'text.secondary',
                borderRadius: '8px',
              }}
            />
          )}
          <Tooltip title={mode === 'dark' ? 'Modo claro' : 'Modo escuro'}>
            <IconButton onClick={toggleColorMode} size="small" sx={{ color: 'text.secondary' }}>
              {mode === 'dark' ? <LightModeIcon fontSize="small" /> : <DarkModeIcon fontSize="small" />}
            </IconButton>
          </Tooltip>
        </Box>
      </TopBar>

      {/* ── Main content ───────────────────────────────────────────────── */}
      <ContentWrapper>
        <ContentCard>
          {/* Hero header */}
          <HeroSection>
            <IconBadge>
              <GavelIcon sx={{ fontSize: 30, color: 'primary.main' }} />
            </IconBadge>

            <Box>
              <Typography
                variant="h1"
                sx={{
                  fontSize: { xs: '2rem', sm: '2.6rem' },
                  color: 'text.primary',
                  mb: 1,
                  lineHeight: 1.15,
                }}
              >
                Termos de Uso
              </Typography>
              <Typography variant="body1" color="text.secondary" sx={{ maxWidth: 560 }}>
                Leia atentamente as condições abaixo antes de utilizar os serviços da plataforma
                Nievo Easy Fin.
              </Typography>
            </Box>
          </HeroSection>

          <Divider />

          {/* Loading state */}
          {isLoading && (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 10 }}>
              <CircularProgress color="primary" />
            </Box>
          )}

          {/* Error state */}
          {!isLoading && error && (
            <Alert severity="error" sx={{ borderRadius: 3 }}>
              {error}
            </Alert>
          )}

          {/* Content — sanitised by dangerouslySetInnerHTML; the backend owns this content */}
          {!isLoading && !error && content && (
            <ProseContainer
              dangerouslySetInnerHTML={{ __html: content }}
            />
          )}

          {/* Empty fallback */}
          {!isLoading && !error && !content && (
            <Alert severity="info" sx={{ borderRadius: 3 }}>
              Os termos de uso não estão disponíveis no momento.
            </Alert>
          )}

          {/* Footer stamp */}
          {!isLoading && !error && (
            <Box sx={{ textAlign: 'center', pb: 2 }}>
              <Typography variant="caption" color="text.secondary">
                © {new Date().getFullYear()} Nievo Easy Fin — Todos os direitos reservados.
              </Typography>
            </Box>
          )}
        </ContentCard>
      </ContentWrapper>
    </PageRoot>
  );
};

export default TermsView;
