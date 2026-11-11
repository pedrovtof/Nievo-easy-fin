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
import CreditCardIcon from '@mui/icons-material/CreditCard';
import { useText } from '../../hooks/useText';
import {
  CardsWrapper,
  FilterStrip,
  WalletGrid,
  CreditCardVisual,
  CatalogRow,
} from './styles';

/**
 * Cards View Template
 * Pure presentation layer displaying the user's card wallet, metadata filters, and card catalog.
 *
 * @param {Object} props - Properties passed from the Cards controller.
 * @returns {JSX.Element} The rendered visual layer of the Cards page.
 */
const CardsView = ({
  userCards,
  catalogCards,
  banks,
  cardTypes,
  cardFlags,
  filters,
  loading,
  error,
  modalOpen,
  submitting,
  feedback,
  formData,
  onFilterChange,
  onOpenModal,
  onCloseModal,
  onFormChange,
  onSubmit,
}) => {
  const { t } = useText();

  return (
    <CardsWrapper>
      <Box display="flex" justifyContent="space-between" alignItems="flex-end" flexWrap="wrap" gap={2}>
        <Box maxWidth={620}>
          <Typography variant="h2" color="primary">
            {t('Cards.title')}
          </Typography>
          <Typography variant="body1" color="secondary" mt={0.5}>
            {t('Cards.subtitle')}
          </Typography>
        </Box>
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<AddIcon />}
          onClick={() => onOpenModal()}
        >
          {t('Cards.addCard')}
        </Button>
      </Box>

      <FilterStrip>
        <Typography variant="caption" color="secondary" fontWeight={700} sx={{ textTransform: 'uppercase', letterSpacing: 1 }}>
          Filter Wallet:
        </Typography>
        <TextField
          select
          size="small"
          label="Institution"
          value={filters.bank_id}
          onChange={(e) => onFilterChange('bank_id', e.target.value)}
          sx={{ minWidth: 180 }}
        >
          <MenuItem value="">All Banks</MenuItem>
          {banks.map((b) => (
            <MenuItem key={b.bank_id} value={b.bank_id}>
              {b.name}
            </MenuItem>
          ))}
        </TextField>

        <TextField
          select
          size="small"
          label="Card Network (Flag)"
          value={filters.flag}
          onChange={(e) => onFilterChange('flag', e.target.value)}
          sx={{ minWidth: 180 }}
        >
          <MenuItem value="">All Flags</MenuItem>
          {cardFlags.map((f) => (
            <MenuItem key={f.name} value={f.name}>
              {f.name}
            </MenuItem>
          ))}
        </TextField>

        <Box ml="auto" display="flex" gap={1} flexWrap="wrap">
          {cardTypes.map((ct) => (
            <Chip
              key={ct.id}
              label={`${ct.name} Supported`}
              size="small"
              sx={{ bgcolor: 'surface.highest', color: 'text.secondary', fontWeight: 500 }}
            />
          ))}
        </Box>
      </FilterStrip>

      {error && <Alert severity="error">{error}</Alert>}

      <Box>
        <Typography variant="h3" color="primary" mb={3}>
          {t('Cards.myCards')}
        </Typography>
        {loading ? (
          <Box display="flex" justifyContent="center" p={5}>
            <CircularProgress />
          </Box>
        ) : userCards.length === 0 ? (
          <Box p={4} bgcolor="surface.main" borderRadius={3}>
            <Typography variant="body1" color="secondary">
              No cards found matching your filter criteria. Add a card from the catalog to get started.
            </Typography>
          </Box>
        ) : (
          <WalletGrid>
            {userCards.map((card, index) => {
              const isPrimary = index === 0;
              const expDate = card.expired_at
                ? new Date(card.expired_at).toLocaleDateString('en-US', { month: '2-digit', year: '2-digit' })
                : '12/29';
              return (
                <CreditCardVisual key={card.user_bank_card_id || index} primaryCard={isPrimary ? 1 : 0}>
                  <Box display="flex" justifyContent="space-between" alignItems="flex-start">
                    <Box>
                      <Typography variant="caption" sx={{ opacity: 0.8, textTransform: 'uppercase', letterSpacing: 1 }}>
                        {card.bank_name}
                      </Typography>
                      <Typography variant="h3" sx={{ fontSize: '1.35rem', mt: 0.5, color: isPrimary ? '#fff' : 'primary.main' }}>
                        {card.user_bank_card_nickname}
                      </Typography>
                    </Box>
                    <CreditCardIcon sx={{ opacity: 0.85 }} />
                  </Box>

                  <Box my={2}>
                    <Typography variant="body2" sx={{ opacity: 0.85 }}>
                      {card.bank_card_name}
                    </Typography>
                  </Box>

                  <Box display="flex" justifyContent="space-between" alignItems="flex-end">
                    <Box>
                      <Typography variant="caption" sx={{ opacity: 0.75, display: 'block' }}>
                        EXPIRES
                      </Typography>
                      <Typography variant="subtitle1" fontWeight={700}>
                        {expDate}
                      </Typography>
                    </Box>
                    <Box display="flex" gap={1}>
                      <Chip
                        label={card.bank_card_type}
                        size="small"
                        sx={{
                          bgcolor: isPrimary ? 'rgba(255,255,255,0.2)' : 'surface.highest',
                          color: isPrimary ? '#fff' : 'primary.main',
                          fontWeight: 600,
                        }}
                      />
                      <Chip
                        label={card.bank_card_flag}
                        size="small"
                        sx={{
                          bgcolor: isPrimary ? 'rgba(255,255,255,0.2)' : 'surface.highest',
                          color: isPrimary ? '#fff' : 'tertiary.main',
                          fontWeight: 700,
                        }}
                      />
                    </Box>
                  </Box>
                </CreditCardVisual>
              );
            })}
          </WalletGrid>
        )}
      </Box>

      <Box>
        <Typography variant="h3" color="primary" mb={3}>
          {t('Cards.catalog')}
        </Typography>
        <Box display="flex" flexDirection="column" gap={2}>
          {catalogCards.map((item) => (
            <CatalogRow key={item.id}>
              <Box display="flex" alignItems="center" gap={2}>
                <Box
                  sx={{
                    width: 44,
                    height: 44,
                    borderRadius: 3,
                    bgcolor: 'surface.high',
                    color: 'primary.main',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                  }}
                >
                  <CreditCardIcon fontSize="small" />
                </Box>
                <Box>
                  <Typography variant="subtitle1" fontWeight={700} color="primary">
                    {item.card_name}
                  </Typography>
                  <Typography variant="body2" color="secondary">
                    {item.bank} • {item.card_type}
                  </Typography>
                </Box>
              </Box>
              <Box display="flex" alignItems="center" gap={2}>
                <Chip label={item.card_flag} size="small" sx={{ bgcolor: 'surface.high', fontWeight: 600 }} />
                <Button variant="contained" color="secondary" onClick={() => onOpenModal(item)}>
                  Add to Wallet
                </Button>
              </Box>
            </CatalogRow>
          ))}
        </Box>
      </Box>

      <Dialog open={modalOpen} onClose={onCloseModal} maxWidth="sm" fullWidth>
        <form onSubmit={onSubmit}>
          <DialogTitle sx={{ fontFamily: '"Manrope", sans-serif', fontWeight: 700 }}>
            {t('Cards.addCard')}
          </DialogTitle>
          <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 3, pt: 2 }}>
            {feedback && <Alert severity={feedback.type}>{feedback.message}</Alert>}

            <TextField
              select
              label="Financial Institution"
              value={formData.bank_id}
              onChange={(e) => onFormChange('bank_id', e.target.value)}
              fullWidth
              required
            >
              {banks.map((b) => (
                <MenuItem key={b.bank_id} value={b.bank_id}>
                  {b.name}
                </MenuItem>
              ))}
            </TextField>

            <TextField
              select
              label="Card Product"
              value={formData.card_id}
              onChange={(e) => onFormChange('card_id', e.target.value)}
              fullWidth
              required
            >
              {catalogCards.map((c) => (
                <MenuItem key={c.id} value={c.id}>
                  {c.card_name} ({c.bank} - {c.card_flag})
                </MenuItem>
              ))}
            </TextField>

            <TextField
              label="Card Nickname in Wallet"
              placeholder="e.g., Family Household Credit Card"
              value={formData.nickname}
              onChange={(e) => onFormChange('nickname', e.target.value)}
              fullWidth
              required
            />

            <TextField
              label="Expiration Date"
              type="date"
              value={formData.expire_at}
              onChange={(e) => onFormChange('expire_at', e.target.value)}
              InputLabelProps={{ shrink: true }}
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
    </CardsWrapper>
  );
};

export default CardsView;
