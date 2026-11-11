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
  RadioGroup,
  FormControlLabel,
  Radio,
  FormLabel,
  Alert,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import CategoryIcon from '@mui/icons-material/Category';
import SubdirectoryArrowRightIcon from '@mui/icons-material/SubdirectoryArrowRight';
import FlagIcon from '@mui/icons-material/Flag';
import { useText } from '../../hooks/useText';
import {
  CategoriesWrapper,
  StatsGrid,
  StatCard,
  CategoryList,
  CategoryRow,
} from './styles';

/**
 * Categories View Template
 * Pure presentation layer displaying the user's categories and the creation modal with exclusive linkage.
 *
 * @param {Object} props - Properties passed from the Categories controller.
 * @returns {JSX.Element} The rendered visual layer of the Categories page.
 */
const CategoriesView = ({
  categories,
  goals,
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

  const rootCount = categories.filter((c) => !c.parent_category).length;
  const subCount = categories.filter((c) => Boolean(c.parent_category)).length;

  return (
    <CategoriesWrapper>
      <Box display="flex" justifyContent="space-between" alignItems="flex-end" flexWrap="wrap" gap={2}>
        <Box maxWidth={620}>
          <Typography variant="h2" color="primary">
            {t('Categories.title')}
          </Typography>
          <Typography variant="body1" color="secondary" mt={0.5}>
            {t('Categories.subtitle')}
          </Typography>
        </Box>
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<AddIcon />}
          onClick={onOpenModal}
        >
          {t('Categories.addCategory')}
        </Button>
      </Box>

      <StatsGrid>
        <StatCard>
          <Typography variant="caption" color="secondary" sx={{ textTransform: 'uppercase', letterSpacing: 1 }}>
            {t('Categories.totalCategories')}
          </Typography>
          <Typography variant="h2" color="primary">
            {categories.length}
          </Typography>
        </StatCard>
        <StatCard>
          <Typography variant="caption" color="secondary" sx={{ textTransform: 'uppercase', letterSpacing: 1 }}>
            {t('Categories.linkedToGoal')}
          </Typography>
          <Typography variant="h2" color="primary">
            {rootCount}
          </Typography>
        </StatCard>
        <StatCard>
          <Typography variant="caption" color="secondary" sx={{ textTransform: 'uppercase', letterSpacing: 1 }}>
            {t('Categories.subcategories')}
          </Typography>
          <Typography variant="h2" color="tertiary.main">
            {subCount}
          </Typography>
        </StatCard>
      </StatsGrid>

      <Box>
        <Typography variant="h3" color="primary" mb={3}>
          Category Hierarchy
        </Typography>
        {categories.length === 0 ? (
          <Box p={4} bgcolor="surface.main" borderRadius={3}>
            <Typography variant="body1" color="secondary">
              No categories registered yet. Create a category linked to one of your financial goals.
            </Typography>
          </Box>
        ) : (
          <CategoryList>
            {categories.map((cat, idx) => {
              const parentObj = cat.parent_category
                ? categories.find((p) => Number(p.id) === Number(cat.parent_category))
                : null;

              return (
                <CategoryRow key={cat.id || idx} alternate={idx % 2 === 1 ? 1 : 0}>
                  <Box display="flex" alignItems="center" gap={2}>
                    <Box
                      sx={{
                        width: 44,
                        height: 44,
                        borderRadius: 3,
                        bgcolor: 'surface.high',
                        color: cat.parent_category ? 'tertiary.main' : 'primary.main',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                      }}
                    >
                      {cat.parent_category ? (
                        <SubdirectoryArrowRightIcon fontSize="small" />
                      ) : (
                        <CategoryIcon fontSize="small" />
                      )}
                    </Box>
                    <Box>
                      <Typography variant="subtitle1" fontWeight={700} color="primary">
                        {cat.name}
                      </Typography>
                      <Typography variant="body2" color="secondary">
                        {cat.description || 'Financial classification'}
                      </Typography>
                    </Box>
                  </Box>

                  <Box display="flex" alignItems="center" gap={1.5}>
                    {cat.goal_name && (
                      <Chip
                        icon={<FlagIcon sx={{ fontSize: 16 }} />}
                        label={`Goal: ${cat.goal_name}`}
                        size="small"
                        sx={{
                          bgcolor: 'surface.high',
                          color: 'primary.main',
                          fontWeight: 600,
                        }}
                      />
                    )}
                    {cat.parent_category && (
                      <Chip
                        label={`Parent: ${parentObj ? parentObj.name : `#${cat.parent_category}`}`}
                        size="small"
                        sx={{
                          bgcolor: 'surface.highest',
                          color: 'tertiary.main',
                          fontWeight: 600,
                        }}
                      />
                    )}
                  </Box>
                </CategoryRow>
              );
            })}
          </CategoryList>
        )}
      </Box>

      <Dialog open={modalOpen} onClose={onCloseModal} maxWidth="sm" fullWidth>
        <form onSubmit={onSubmit}>
          <DialogTitle sx={{ fontFamily: '"Manrope", sans-serif', fontWeight: 700 }}>
            {t('Categories.addCategory')}
          </DialogTitle>
          <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 3, pt: 2 }}>
            <Typography variant="body2" color="secondary">
              Each category must be exclusively linked to either a Financial Goal or an existing Parent Category.
            </Typography>
            {feedback && <Alert severity={feedback.type}>{feedback.message}</Alert>}

            <TextField
              label="Category Name"
              placeholder="e.g., Groceries & Market"
              value={formData.name}
              onChange={(e) => onFormChange('name', e.target.value)}
              fullWidth
              required
            />

            <TextField
              label="Description"
              placeholder="Brief note about what belongs in this category"
              value={formData.description}
              onChange={(e) => onFormChange('description', e.target.value)}
              fullWidth
            />

            <Box>
              <FormLabel sx={{ fontWeight: 600, fontSize: '0.875rem' }}>
                Linkage Type (Exclusive)
              </FormLabel>
              <RadioGroup
                row
                value={formData.linkMode}
                onChange={(e) => onFormChange('linkMode', e.target.value)}
              >
                <FormControlLabel value="goal" control={<Radio />} label="Link to Financial Goal" />
                <FormControlLabel
                  value="parent"
                  control={<Radio />}
                  label="Link to Parent Category"
                  disabled={categories.length === 0}
                />
              </RadioGroup>
            </Box>

            {formData.linkMode === 'goal' ? (
              <TextField
                select
                label="Target Financial Goal"
                value={formData.goalId}
                onChange={(e) => onFormChange('goalId', e.target.value)}
                fullWidth
                required
              >
                {goals.map((g) => (
                  <MenuItem key={g.id} value={g.id}>
                    {g.name} ({g.is_percent ? `${g.amount}%` : `$${g.amount}`})
                  </MenuItem>
                ))}
              </TextField>
            ) : (
              <TextField
                select
                label="Parent Category"
                value={formData.parentCategoryId}
                onChange={(e) => onFormChange('parentCategoryId', e.target.value)}
                fullWidth
                required
              >
                {categories.map((c) => (
                  <MenuItem key={c.id} value={Number(c.id)}>
                    {c.name}
                  </MenuItem>
                ))}
              </TextField>
            )}
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
    </CategoriesWrapper>
  );
};

export default CategoriesView;
