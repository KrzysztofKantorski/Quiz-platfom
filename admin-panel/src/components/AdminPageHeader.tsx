import { Box, Typography, Button } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

interface AdminPageHeaderProps {
  title: string;
  actionText?: string;
  onAction?: () => void;
}

export const AdminPageHeader = ({ title, actionText, onAction }: AdminPageHeaderProps) => {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 4 }}>
      <Typography variant="h4" color="text.primary">
        {title}
      </Typography>
      {actionText && onAction && (
        <Button 
          variant="contained" 
          startIcon={<AddIcon />}
          onClick={onAction}
        >
          {actionText}
        </Button>
      )}
    </Box>
  );
};