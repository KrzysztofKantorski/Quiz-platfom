import { useNavigate } from 'react-router-dom';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { 
  Box, TableCell,TableRow,
 CircularProgress, Alert,
 Button
} from '@mui/material';

import { deleteQuiz, getQuizzes } from '../api/quizzes';
import { AdminPageHeader } from '../components/AdminPageHeader';
import { AdminActionButtons } from '../components/AdminActionButtons';
import { AdminTable } from '../components/AdminTable';


import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
export const QuizList = () => {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  //Get quizzes
  const { data: quizzes, isLoading, isError, error } = useQuery({
    queryKey: ['quizzes'],
    queryFn: getQuizzes
  });

  //Delete quiz
  const deleteMutation = useMutation({
    mutationFn: deleteQuiz,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['quizzes'] });
    }
  });

  //Delete habdler
  const handleDelete = (id: number) => {
    if (window.confirm('Are you sure you want to delete this quiz?')) {
      deleteMutation.mutate(id);
    }
  };

  return (
    <Box>
      <AdminPageHeader 
        title="Quiz management" 
        actionText="Add new quiz" 
        onAction={() => navigate('/quizzes/new')} 
      />

      {isLoading && 
      <CircularProgress
       sx={{
         display: 'block',
          margin: '40px auto' 
        }} 
       />
      }
      {isError && 
        <Alert severity="error">
          An error occured: {(error as Error).message}
        </Alert>
      }

      {!isLoading && !isError && quizzes && (
        <AdminTable headers={['Title', 'Description', 'Questions', 'Action']}>
          {quizzes.length === 0 ? (
            <TableRow>
              <TableCell colSpan={4} align="center" sx={{ py: 3 }}>
               No quizzes in db
              </TableCell>
            </TableRow>
          ) : (
            quizzes.map((quiz) => (
              <TableRow key={quiz.id}>
                <TableCell >{quiz.title}</TableCell>
                <TableCell>{quiz.description}</TableCell>
                <TableCell>
                  <Button 
                    title="Edit questions"
                    size="small" 
                    sx={{ mr: 1 }}
                    onClick={() => navigate(`/quizzes/${quiz.id}/questions`)}
                  >
                  <FormatListBulletedIcon />
                  </Button>
                </TableCell>
                <TableCell align="right">
                  
                  <AdminActionButtons 
                    onEdit={() => navigate(`/quizzes/${quiz.id}/edit`)} 
                    onDelete={() => handleDelete(quiz.id)} 
                  />
                </TableCell>
              </TableRow>
            ))
          )}
        </AdminTable>
      )}
    </Box>
  );
};