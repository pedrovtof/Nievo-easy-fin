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
  Grid,
  Alert,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import NorthEastIcon from '@mui/icons-material/NorthEast';
import SouthWestIcon from '@mui/icons-material/SouthWest';
import AutorenewIcon from '@mui/icons-material/Autorenew';
import { useText } from '../../hooks/useText';
import {
  TransactionsWrapper,
  FiltersBar,
  TransactionsListContainer,
  TransactionRow,
} from './styles';

/**
 * Transactions View Template
 * Pure presentation component to display a filterable list of transactions and the creation dialog.
 *
 * @param {Object} props - Contains transactions data, metadata lists, filters, and modal handlers.
 * @returns {JSX.Element} The rendered visual layer of the Transactions page.
 */
const TransactionsView = ({
  transactions,
  statuses,
  situations = [],
  types,
  schedulers,
  categories,
  banks,
  cards,
  filters,
  loading,
  error,
  modalOpen,
  submitting,
  feedback,
  formData,
  onFilterChange,
  onResetFilters,
  onOpenModal,
  onCloseModal,
  onFormChange,
  onSubmit,
}) => {
  const { t } = useText();

  return (
    <TransactionsWrapper>
      <Box display="flex" justifyContent="space-between" alignItems="flex-end" flexWrap="wrap" gap={2}>
        <Box maxWidth={620}>
          <Typography variant="h2" color="primary">
            {t('Transactions.title')}
          </Typography>
          <Typography variant="body1" color="secondary" mt={0.5}>
            {t('Transactions.subtitle')}
          </Typography>
        </Box>
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<AddIcon />}
          onClick={onOpenModal}
        >
          {t('Transactions.addTransaction')}
        </Button>
      </Box>

      <FiltersBar>
        <Typography
          variant="caption"
          fontWeight={700}
          color="secondary"
          sx={{ textTransform: 'uppercase', letterSpacing: 1 }}
        >
          Filters:
        </Typography>

        <TextField
          select
          size="small"
          label="Status"
          value={filters.transaction_status_id}
          onChange={(e) => onFilterChange('transaction_status_id', e.target.value)}
          sx={{ minWidth: 150 }}
        >
          <MenuItem value="">All Statuses</MenuItem>
          {statuses.map((st) => (
            <MenuItem key={st.id} value={st.id}>
              {st.name}
            </MenuItem>
          ))}
        </TextField>

        <TextField
          select
          size="small"
          label="Situation"
          value={filters.transaction_situation_id || ''}
          onChange={(e) => onFilterChange('transaction_situation_id', e.target.value)}
          sx={{ minWidth: 150 }}
        >
          <MenuItem value="">All Situations</MenuItem>
          {situations.map((sit) => (
            <MenuItem key={sit.id} value={sit.id}>
              {sit.name}
            </MenuItem>
          ))}
        </TextField>

        <TextField
          select
          size="small"
          label="Type"
          value={filters.transaction_type_id}
          onChange={(e) => onFilterChange('transaction_type_id', e.target.value)}
          sx={{ minWidth: 150 }}
        >
          <MenuItem value="">All Types</MenuItem>
          {types.map((tp) => (
            <MenuItem key={tp.id} value={tp.id}>
              {tp.name}
            </MenuItem>
          ))}
        </TextField>

        <TextField
          select
          size="small"
          label="Category"
          value={filters.category}
          onChange={(e) => onFilterChange('category', e.target.value)}
          sx={{ minWidth: 170 }}
        >
          <MenuItem value="">All Categories</MenuItem>
          {categories.map((cat) => (
            <MenuItem key={cat.id} value={cat.id}>
              {cat.name}
            </MenuItem>
          ))}
        </TextField>

        <TextField
          select
          size="small"
          label="Card"
          value={filters.user_bank_card}
          onChange={(e) => onFilterChange('user_bank_card', e.target.value)}
          sx={{ minWidth: 170 }}
        >
          <MenuItem value="">All Cards</MenuItem>
          {cards.map((c) => (
            <MenuItem key={c.user_bank_card_id} value={c.user_bank_card_id}>
              {c.user_bank_card_nickname}
            </MenuItem>
          ))}
        </TextField>

        <Button variant="text" color="secondary" onClick={onResetFilters} sx={{ ml: 'auto' }}>
          Clear Filters
        </Button>
      </FiltersBar>

      {error && <Alert severity="error">{error}</Alert>}

      {loading ? (
        <Box display="flex" justifyContent="center" p={6}>
          <CircularProgress />
        </Box>
      ) : transactions.length === 0 ? (
        <Box p={5} bgcolor="surface.main" borderRadius={3} textAlign="center">
          <Typography variant="body1" color="secondary">
            No transactions found for the selected filters. Click &ldquo;{t('Transactions.addTransaction')}&rdquo; to record a new entry.
          </Typography>
        </Box>
      ) : (
        <TransactionsListContainer>
          {transactions.map((tx, idx) => {
            const sitLower = tx.situation?.toLowerCase();
            const isIncome =
              sitLower === 'entrada' ||
              sitLower === 'income' ||
              (!sitLower &&
                (tx.type?.toLowerCase() === 'income' || tx.type?.toLowerCase() === 'receita'));
            const formattedDate = tx.pay_date
              ? new Date(tx.pay_date).toLocaleDateString('en-US', {
                  month: 'short',
                  day: 'numeric',
                  year: 'numeric',
                })
              : '';

            return (
              <TransactionRow key={tx.id || idx} alternate={idx % 2 === 1 ? 1 : 0}>
                <Box display="flex" alignItems="center" gap={2.5} minWidth={240}>
                  <Box
                    sx={{
                      width: 44,
                      height: 44,
                      borderRadius: 3,
                      bgcolor: isIncome ? 'success.container' : 'surface.high',
                      color: isIncome ? 'success.main' : 'primary.main',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                    }}
                  >
                    {isIncome ? <SouthWestIcon fontSize="small" /> : <NorthEastIcon fontSize="small" />}
                  </Box>
                  <Box>
                    <Typography variant="subtitle1" fontWeight={700} color="primary">
                      {tx.name}
                    </Typography>
                    <Typography variant="body2" color="secondary">
                      {tx.description}
                    </Typography>
                  </Box>
                </Box>

                <Box display="flex" alignItems="center" gap={1.5} flexWrap="wrap">
                  {tx.situation && (
                    <Chip
                      label={tx.situation}
                      size="small"
                      sx={{
                        bgcolor: isIncome ? 'success.container' : 'error.container',
                        color: isIncome ? 'success.main' : 'error.main',
                        fontWeight: 600,
                      }}
                    />
                  )}
                  <Chip
                    label={tx.category || 'Uncategorized'}
                    size="small"
                    sx={{ bgcolor: 'surface.high', color: 'primary.main', fontWeight: 600 }}
                  />
                  <Chip
                    label={tx.status || 'Completed'}
                    size="small"
                    sx={{
                      bgcolor: 'surface.highest',
                      color: tx.status?.toLowerCase() === 'pending' ? 'tertiary.main' : 'text.secondary',
                      fontWeight: 600,
                    }}
                  />
                  {tx.cron_tab && (
                    <Chip
                      icon={<AutorenewIcon sx={{ fontSize: 14 }} />}
                      label="Recurring"
                      size="small"
                      sx={{ bgcolor: 'surface.high', color: 'tertiary.main', fontWeight: 600 }}
                    />
                  )}
                </Box>

                <Box textAlign="right" minWidth={180}>
                  <Typography
                    variant="h3"
                    sx={{ fontSize: '1.25rem' }}
                    color={isIncome ? 'success.main' : 'primary.main'}
                  >
                    {isIncome ? '+' : '-'}${Math.abs(Number(tx.amount)).toFixed(2)}
                  </Typography>
                  <Typography variant="caption" color="secondary" display="block">
                    {tx.user_bank_nickname || tx.bank_name}
                    {tx.user_card_name ? ` • ${tx.user_card_name}` : ''} • {formattedDate}
                  </Typography>
                </Box>
              </TransactionRow>
            );
          })}
        </TransactionsListContainer>
      )}

      <Dialog open={modalOpen} onClose={onCloseModal} maxWidth="sm" fullWidth>
        <form onSubmit={onSubmit}>
          <DialogTitle sx={{ fontFamily: '"Manrope", sans-serif', fontWeight: 700 }}>
            {t('Transactions.addTransaction')}
          </DialogTitle>
          <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 2.5, pt: 2 }}>
            {feedback && <Alert severity={feedback.type}>{feedback.message}</Alert>}

            <TextField
              label="Transaction Title"
              placeholder="e.g., Supermarket Shopping"
              value={formData.name}
              onChange={(e) => onFormChange('name', e.target.value)}
              fullWidth
              required
            />

            <TextField
              label="Description"
              placeholder="Detailed note about this payment"
              value={formData.description}
              onChange={(e) => onFormChange('description', e.target.value)}
              fullWidth
              required
            />

            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  label="Amount ($)"
                  type="number"
                  inputProps={{ min: 1 }}
                  value={formData.amount}
                  onChange={(e) => onFormChange('amount', e.target.value)}
                  fullWidth
                  required
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label="Pay Date"
                  type="date"
                  value={formData.pay_date}
                  onChange={(e) => onFormChange('pay_date', e.target.value)}
                  InputLabelProps={{ shrink: true }}
                  fullWidth
                  required
                />
              </Grid>
            </Grid>

            <Grid container spacing={2}>
              <Grid item xs={12} sm={4}>
                <TextField
                  select
                  label="Situation"
                  value={formData.transaction_situation || ''}
                  onChange={(e) => onFormChange('transaction_situation', e.target.value)}
                  fullWidth
                  required
                >
                  {situations.map((sit) => (
                    <MenuItem key={sit.id} value={sit.id}>
                      {sit.name}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
              <Grid item xs={12} sm={4}>
                <TextField
                  select
                  label="Transaction Type"
                  value={formData.transaction_type}
                  onChange={(e) => onFormChange('transaction_type', e.target.value)}
                  fullWidth
                  required
                >
                  {types.map((tp) => (
                    <MenuItem key={tp.id} value={tp.id}>
                      {tp.name}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
              <Grid item xs={12} sm={4}>
                <TextField
                  select
                  label="Status"
                  value={formData.transaction_status}
                  onChange={(e) => onFormChange('transaction_status', e.target.value)}
                  fullWidth
                  required
                >
                  {statuses.map((st) => (
                    <MenuItem key={st.id} value={st.id}>
                      {st.name}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
            </Grid>

            <TextField
              select
              label="Category"
              value={formData.category_name}
              onChange={(e) => onFormChange('category_name', e.target.value)}
              fullWidth
              required
            >
              {categories.map((cat) => (
                <MenuItem key={cat.id} value={cat.name}>
                  {cat.name}
                </MenuItem>
              ))}
            </TextField>

            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  select
                  label="Bank Account"
                  value={formData.bank}
                  onChange={(e) => onFormChange('bank', e.target.value)}
                  fullWidth
                  required
                >
                  {banks.map((b, i) => (
                    <MenuItem key={`${b.resolvedBankId}-${i}`} value={b.resolvedBankId}>
                      {b.nick_name || b.name}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  select
                  label="Card (Optional)"
                  value={formData.card}
                  onChange={(e) => onFormChange('card', e.target.value)}
                  fullWidth
                >
                  <MenuItem value={0}>Direct Account (No Card)</MenuItem>
                  {cards.map((c) => (
                    <MenuItem key={c.user_bank_card_id} value={c.user_bank_card_id}>
                      {c.user_bank_card_nickname}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
            </Grid>

            <TextField
              select
              label="Recurrence Schedule"
              value={formData.scheduler}
              onChange={(e) => onFormChange('scheduler', e.target.value)}
              fullWidth
            >
              <MenuItem value={0}>No Recurrence (One-Time)</MenuItem>
              {schedulers.map((sch) => (
                <MenuItem key={sch.id} value={sch.id}>
                  {sch.name}
                </MenuItem>
              ))}
            </TextField>
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
    </TransactionsWrapper>
  );
};

export default TransactionsView;
