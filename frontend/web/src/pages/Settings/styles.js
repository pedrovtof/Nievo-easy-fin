import { styled } from '@mui/material/styles';
import { Box, Card } from '@mui/material';

/**
 * Root wrapper for the Settings page following The Silent Navigator spacing scale.
 */
export const SettingsContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(4.5),
}));

/**
 * Editorial hero summary banner displaying user identity and status without 1px borders.
 */
export const ProfileHeroCard = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3.5, 4),
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  flexWrap: 'wrap',
  gap: theme.spacing(2.5),
  background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dim || '#3e4652'} 100%)`,
  color: '#ffffff',
  border: 'none',
}));

/**
 * Tonal surface card replacing 1px borders with background layering.
 */
export const SectionCard = styled(Card)(({ theme }) => ({
  padding: theme.spacing(4),
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(3),
  backgroundColor: theme.palette.background.paper,
  border: 'none',
}));

/**
 * Section header using tonal icon badge and whitespace instead of a 1px bottom border.
 */
export const SectionHeader = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  gap: theme.spacing(2),
}));

/**
 * Tonal badge container for section icons.
 */
export const IconBadge = styled(Box)(({ theme }) => ({
  width: 44,
  height: 44,
  borderRadius: 14,
  backgroundColor: theme.palette.surface.high,
  color: theme.palette.primary.main,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
}));

/**
 * Tonal row surface for read-only user detail metadata.
 */
export const DetailFieldBox = styled(Box)(({ theme }) => ({
  padding: theme.spacing(2, 2.5),
  borderRadius: 16,
  backgroundColor: theme.palette.surface.main,
  display: 'flex',
  flexDirection: 'column',
  gap: theme.spacing(0.5),
}));
