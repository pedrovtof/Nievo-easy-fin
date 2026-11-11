import React from 'react';
import {
  Typography,
  Box,
  CircularProgress,
  Button,
  Chip,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  MenuItem,
  Alert,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import AccountBalanceIcon from '@mui/icons-material/AccountBalance';
import VerifiedUserIcon from '@mui/icons-material/VerifiedUser';
import { useText } from '../../hooks/useText';
import {
  BanksWrapper,
  HeroGrid,
  HeroCard,
  AccountsGrid,
  AccountCard,
  CatalogCard,
  IconBadge,
} from './styles';

/**
 * Banks View Template
 * Pure presentation layer displaying the user's linked bank accounts and the supported institution catalog.
 *
 * @param {Object} props - Properties passed from the Banks controller.
 * @returns {JSX.Element} The rendered visual layer of the Banks page.
 */
const BanksView = ({
  userBanks,
  catalogBanks,
  loading,
  error,
  modalOpen,
  submitting,
  feedback,
  formData,
  onOpenModal,
  onCloseModal,
  onFormChange,
  onSubmit,
}) => {
  const { t } = useText();

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" p={6}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }

  return (
    <BanksWrapper>
      <Box display="flex" justifyContent="space-between" alignItems="flex-end" flexWrap="wrap" gap={2}>
        <Box maxWidth={600}>
          <Typography variant="h2" color="primary">
            {t('Banks.title')}
          </Typography>
          <Typography variant="body1" color="secondary" mt={0.5}>
            {t('Banks.subtitle')}
          </Typography>
        </Box>
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<AddIcon />}
          onClick={() => onOpenModal()}
        >
          {t('Banks.addBank')}
        </Button>
      </Box>

      <HeroGrid>
        <HeroCard featured={1}>
          <Typography variant="caption" sx={{ opacity: 0.85, textTransform: 'uppercase', letterSpacing: 1 }}>
            {t('Banks.myAccounts')}
          </Typography>
          <Typography variant="h1" sx={{ color: '#ffffff' }}>
            {userBanks.length}
          </Typography>
          <Typography variant="body2" sx={{ opacity: 0.85 }}>
            Active institutions connected to your profile
          </Typography>
        </HeroCard>
        <HeroCard>
          <Typography variant="caption" color="secondary" sx={{ textTransform: 'uppercase', letterSpacing: 1 }}>
            {t('Banks.catalog')}
          </Typography>
          <Typography variant="h2" color="primary">
            {catalogBanks.length}
          </Typography>
          <Typography variant="body2" color="secondary">
            Verified banking institutions available for linking
          </Typography>
        </HeroCard>
      </HeroGrid>

      <Box>
        <Typography variant="h3" color="primary" mb={3}>
          {t('Banks.myAccounts')}
        </Typography>
        {userBanks.length === 0 ? (
          <Box p={4} bgcolor="surface.main" borderRadius={3}>
            <Typography variant="body1" color="secondary">
              You have not linked any bank accounts yet. Click &ldquo;{t('Banks.addBank')}&rdquo; to connect your first institution.
            </Typography>
          </Box>
        ) : (
          <AccountsGrid>
            {userBanks.map((bank, idx) => (
              <AccountCard key={`${bank.name}-${idx}`}>
                <Box display="flex" alignItems="center" gap={2}>
                  <IconBadge>
                    <AccountBalanceIcon />
                  </IconBadge>
                  <Box>
                    <Typography variant="subtitle1" fontWeight={700} color="primary">
                      {bank.nick_name || bank.name}
                    </Typography>
                    <Typography variant="body2" color="secondary">
                      {bank.name}
                    </Typography>
                  </Box>
                </Box>
                <Box display="flex" justifyContent="space-between" alignItems="center" pt={1}>
                  <Chip
                    label={bank.bank_type || 'Standard Account'}
                    size="small"
                    sx={{
                      bgcolor: 'surface.high',
                      color: 'primary.main',
                      fontWeight: 600,
                    }}
                  />
                  <Box display="flex" alignItems="center" gap={0.5} color="tertiary.main">
                    <VerifiedUserIcon fontSize="small" />
                    <Typography variant="caption" fontWeight={600}>
                      Synced
                    </Typography>
                  </Box>
                </Box>
              </AccountCard>
            ))}
          </AccountsGrid>
        )}
      </Box>

      <Box>
        <Typography variant="h3" color="primary" mb={3}>
          {t('Banks.catalog')}
        </Typography>
        <Box display="flex" flexDirection="column" gap={2}>
          {catalogBanks.map((inst) => {
            const isLinked = userBanks.some(
              (ub) => ub.name?.toLowerCase() === inst.name?.toLowerCase()
            );
            return (
              <CatalogCard key={inst.bank_id}>
                <Box display="flex" alignItems="center" gap={2}>
                  <IconBadge>
                    <AccountBalanceIcon fontSize="small" />
                  </IconBadge>
                  <Box>
                    <Typography variant="subtitle1" fontWeight={700} color="primary">
                      {inst.name}
                    </Typography>
                    <Typography variant="body2" color="secondary">
                      {inst.bank_description || inst.bank_type_name}
                    </Typography>
                  </Box>
                </Box>
                <Box display="flex" alignItems="center" gap={2}>
                  <Chip
                    label={inst.bank_type_name}
                    size="small"
                    sx={{ bgcolor: 'surface.highest', fontWeight: 500 }}
                  />
                  <Button
                    variant={isLinked ? 'contained' : 'outlined'}
                    color={isLinked ? 'secondary' : 'primary'}
                    disabled={isLinked}
                    onClick={() => onOpenModal(inst.bank_id)}
                  >
                    {isLinked ? 'Linked' : 'Connect'}
                  </Button>
                </Box>
              </CatalogCard>
            );
          })}
        </Box>
      </Box>

      <Dialog open={modalOpen} onClose={onCloseModal} maxWidth="sm" fullWidth>
        <form onSubmit={onSubmit}>
          <DialogTitle sx={{ fontFamily: '"Manrope", sans-serif', fontWeight: 700 }}>
            {t('Banks.addBank')}
          </DialogTitle>
          <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 3, pt: 2 }}>
            <Typography variant="body2" color="secondary">
              Select a supported institution from the catalog and assign a personal nickname for easy identification.
            </Typography>
            {feedback && <Alert severity={feedback.type}>{feedback.message}</Alert>}

            <TextField
              select
              label="Financial Institution"
              value={formData.selectedBankId}
              onChange={(e) => onFormChange('selectedBankId', e.target.value)}
              fullWidth
              required
            >
              {catalogBanks.map((bank) => (
                <MenuItem key={bank.bank_id} value={bank.bank_id}>
                  {bank.name} ({bank.bank_type_name})
                </MenuItem>
              ))}
            </TextField>

            <TextField
              label="Account Nickname"
              placeholder="e.g., Family Checking Account"
              value={formData.nickname}
              onChange={(e) => onFormChange('nickname', e.target.value)}
              fullWidth
              required
            />
          </DialogContent>
          <DialogActions sx={{ p: 3 }}>
            <Button onClick={onCloseModal} color="secondary">
              {t('Common.cancel')}
            </Button>
            <Button type="submit" variant="contained" color="primary" disabled={submitting}>
              {submitting ? t('Common.loading') : t('Common.save')}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </BanksWrapper>
  );
};

export default BanksView;
