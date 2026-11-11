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
  FormControlLabel,
  Switch,
  Alert,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import FlagIcon from '@mui/icons-material/Flag';
import EventAvailableIcon from '@mui/icons-material/EventAvailable';
import { useText } from '../../hooks/useText';
import {
  BudgetContainer,
  SummaryGrid,
  SummaryCard,
  CategoryGrid,
  CategoryCard,
  StyledLinearProgress,
} from './styles';

/**
 * Budget & Goals View Template
 * Pure presentation layer displaying financial goals, target allocations, and creation modal.
 *
 * @param {Object} props - Properties passed from the Budget controller.
 * @returns {JSX.Element} The rendered visual layer of the Budget page.
 */
const BudgetView = ({
  goals,
  totalMonetaryTarget,
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
    <BudgetContainer>
      <Box display="flex" justifyContent="space-between" alignItems="flex-end" flexWrap="wrap" gap={2}>
        <Box maxWidth={620}>
          <Typography variant="h2" color="primary">
            {t('Budget.title')}
          </Typography>
          <Typography variant="body1" color="secondary" mt={0.5}>
            {t('Budget.subtitle')}
          </Typography>
        </Box>
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<AddIcon />}
          onClick={onOpenModal}
        >
          {t('Budget.addBudget')}
        </Button>
      </Box>

      <SummaryGrid>
        <SummaryCard featured={1}>
          <Typography variant="caption" sx={{ opacity: 0.85, textTransform: 'uppercase', letterSpacing: 1 }}>
            {t('Budget.totalBudget')}
          </Typography>
          <Typography variant="h1" sx={{ color: '#ffffff' }}>
            ${totalMonetaryTarget.toLocaleString('en-US', { minimumFractionDigits: 2 })}
          </Typography>
          <Typography variant="body2" sx={{ opacity: 0.85 }}>
            Combined monetary goals across active plans
          </Typography>
        </SummaryCard>
        <SummaryCard>
          <Typography variant="caption" color="secondary" sx={{ textTransform: 'uppercase', letterSpacing: 1 }}>
            {t('Budget.activeGoals')}
          </Typography>
          <Typography variant="h2" color="primary">
            {goals.length}
          </Typography>
          <Typography variant="body2" color="secondary">
            Goals available to link with your spending categories
          </Typography>
        </SummaryCard>
      </SummaryGrid>

      <Box>
        <Typography variant="h3" color="primary" mb={3}>
          Active Financial Goals
        </Typography>
        {goals.length === 0 ? (
          <Box p={4} bgcolor="surface.main" borderRadius={3}>
            <Typography variant="body1" color="secondary">
              No active goals yet. Create your first financial goal to organize your category budgets.
            </Typography>
          </Box>
        ) : (
          <CategoryGrid>
            {goals.map((goal) => {
              const expDate = goal.expire_at
                ? new Date(goal.expire_at).toLocaleDateString('en-US', {
                    month: 'short',
                    day: 'numeric',
                    year: 'numeric',
                  })
                : 'No expiration';

              return (
                <CategoryCard key={goal.id}>
                  <Box display="flex" justifyContent="space-between" alignItems="flex-start" gap={2}>
                    <Box display="flex" gap={2} alignItems="center">
                      <Box
                        sx={{
                          width: 44,
                          height: 44,
                          borderRadius: 3,
                          bgcolor: 'surface.high',
                          color: goal.is_percent ? 'tertiary.main' : 'primary.main',
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                        }}
                      >
                        <FlagIcon fontSize="small" />
                      </Box>
                      <Box>
                        <Typography variant="subtitle1" fontWeight={700} color="primary">
                          {goal.name}
                        </Typography>
                        <Typography variant="body2" color="secondary">
                          {goal.description || 'Financial milestone'}
                        </Typography>
                      </Box>
                    </Box>
                    <Chip
                      label={goal.is_percent ? 'Percentage' : 'Fixed Target'}
                      size="small"
                      sx={{
                        bgcolor: 'surface.high',
                        color: goal.is_percent ? 'tertiary.main' : 'primary.main',
                        fontWeight: 600,
                      }}
                    />
                  </Box>

                  <Box mt="auto">
                    <Box display="flex" justifyContent="space-between" alignItems="baseline" mb={1}>
                      <Typography variant="h3" color="primary">
                        {goal.is_percent
                          ? `${goal.amount}%`
                          : `$${Number(goal.amount).toLocaleString('en-US', { minimumFractionDigits: 2 })}`}
                      </Typography>
                      <Typography variant="caption" color="secondary" fontWeight={600}>
                        {t('Budget.spent')}
                      </Typography>
                    </Box>

                    <StyledLinearProgress
                      variant="determinate"
                      value={goal.is_percent ? Math.min(Number(goal.amount), 100) : 100}
                      isPercent={goal.is_percent ? 1 : 0}
                    />

                    <Box display="flex" alignItems="center" gap={0.75} mt={2} color="tertiary.main">
                      <EventAvailableIcon sx={{ fontSize: 16 }} />
                      <Typography variant="caption" fontWeight={600}>
                        {t('Budget.remaining')}: {expDate}
                      </Typography>
                    </Box>
                  </Box>
                </CategoryCard>
              );
            })}
          </CategoryGrid>
        )}
      </Box>

      <Dialog open={modalOpen} onClose={onCloseModal} maxWidth="sm" fullWidth>
        <form onSubmit={onSubmit}>
          <DialogTitle sx={{ fontFamily: '"Manrope", sans-serif', fontWeight: 700 }}>
            {t('Budget.addBudget')}
          </DialogTitle>
          <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 3, pt: 2 }}>
            {feedback && <Alert severity={feedback.type}>{feedback.message}</Alert>}

            <TextField
              label="Goal Name"
              placeholder="e.g., Emergency Reserve"
              value={formData.name}
              onChange={(e) => onFormChange('name', e.target.value)}
              fullWidth
              required
            />

            <TextField
              label="Description"
              placeholder="Describe the purpose of this financial goal"
              value={formData.description}
              onChange={(e) => onFormChange('description', e.target.value)}
              fullWidth
              multiline
              rows={2}
            />

            <TextField
              label={formData.is_percent ? 'Target Percentage (%)' : 'Target Amount ($)'}
              type="number"
              inputProps={{ min: 1 }}
              value={formData.amount}
              onChange={(e) => onFormChange('amount', e.target.value)}
              fullWidth
              required
            />

            <FormControlLabel
              control={
                <Switch
                  checked={formData.is_percent}
                  onChange={(e) => onFormChange('is_percent', e.target.checked)}
                />
              }
              label="Define target as a percentage (%) of income"
            />

            <TextField
              label="Target Completion Date"
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
    </BudgetContainer>
  );
};

export default BudgetView;
