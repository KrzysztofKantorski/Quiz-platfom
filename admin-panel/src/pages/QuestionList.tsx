import { useParams, useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { Box, TableCell, TableRow, CircularProgress, Alert, Button } from '@mui/material';
import { getQuestionsForQuiz, deleteQuestion } from '../api/questions'; 
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import { AdminPageHeader } from '../components/AdminPageHeader';
import { AdminTable } from '../components/AdminTable';
import { AdminActionButtons } from '../components/AdminActionButtons';

export const QuestionList = () => {

  //Quiz id from url
  const { id } = useParams(); 
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  //Get questions for quiz
  const { data: questions, isLoading, isError } = useQuery({
    queryKey: ['questions', id],
    queryFn: () => getQuestionsForQuiz(Number(id)),
  });

  //Delete question
  const deleteMutation = useMutation({
    mutationFn: deleteQuestion,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['questions', id] });
    }
  });

  //Delete handler
  const handleDelete = (questionId: number) => {
    if (window.confirm('Are you sure you want to delete this question with all answers?')) {
      deleteMutation.mutate(questionId);
    }
  };

  if(isError){
    return (
       <Alert severity="error">Failed to load questions.</Alert>
    )
   
  }
  if(isLoading){
    return(
      <CircularProgress sx={{ display: 'block', mx: 'auto', mt: 4 }} />
    )
  }

  return (
    <Box>
      <Box sx={{ mb: 1 }}>
        <Button onClick={() => navigate('/quizzes')} sx={{ m:1,p:1, width:"10px"}}>
          <ArrowBackIosIcon onClick={() => navigate('/quizzes')}/>
        </Button>
      </Box>

      <AdminPageHeader 
        title={`Questions for Quiz`} 
        actionText="Add new question" 
        onAction={() => navigate(`/quizzes/${id}/questions/new`)} 
      />
      {!isLoading && !isError && questions && (
        <AdminTable headers={[ 'Question content', 'Points', 'Answers', 'Actions']}>
          {questions.length === 0 ? (
            <TableRow>
              <TableCell colSpan={4} align="center" sx={{ py: 3 }}>
                No questions yet. Create the first one!
              </TableCell>
            </TableRow>
          ) : (
            questions.map((q) => (
              <TableRow key={q.id}>
                <TableCell>{q.text}</TableCell>
                <TableCell>{q.points}</TableCell>
                <TableCell>{q.answers.length}</TableCell>
                
                <TableCell align="right">
                  <AdminActionButtons 
                    onEdit={() => navigate(`/quizzes/${id}/questions/${q.id}/edit`)} 
                    onDelete={() => handleDelete(q.id)}
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