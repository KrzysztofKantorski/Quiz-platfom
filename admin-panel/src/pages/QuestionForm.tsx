import { useQueryClient, useQuery, useMutation } from '@tanstack/react-query';
import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import type { CreateAnswerDto } from '../types/answers';
import { updateQuestion, getQuestionById, addQuestion } from '../api/questions';
import type { CreateQuestionDto } from '../types/questions';
import { Alert, Box, Checkbox, CircularProgress, FormControlLabel, IconButton, Paper, Typography } from '@mui/material';
import { AdminButton } from '../components/AdminButton';
import { AdminInput } from '../components/AdminInput';
import { AdminPageHeader } from '../components/AdminPageHeader';
import DeleteIcon from '@mui/icons-material/Delete';

export const QuestionForm = ()=>{
    const { id: quizId, questionId } = useParams(); 
    const navigate = useNavigate();
    const queryClient = useQueryClient();
    const isEditMode = Boolean(questionId);

    const [text, setText] = useState('');
    const [points, setPoints] = useState<number>(1);
    const [answers, setAnswers] = useState<CreateAnswerDto[]>([
        { text: '', isCorrect: false },
        { text: '', isCorrect: false } 
    ]);
    const [validationError, setValidationError] = useState('');

    //Get question data
    const { data: questionData, isLoading: isFetching, isError: isFetchError } = useQuery({
      queryKey: ['question', questionId],
      queryFn: () => getQuestionById(Number(questionId)),
      enabled: isEditMode
    });

    //Fill form with question data
    useEffect(() => {
    if (questionData) {
      setText(questionData.text);
      setPoints(questionData.points);
      setAnswers(questionData.answers || []);
    }
    }, [questionData]);


    //Create new answer
    const createMutation = useMutation({
    mutationFn: (payload: CreateQuestionDto) => addQuestion(Number(quizId), payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['questions', quizId] });
      navigate(`/quizzes/${quizId}/questions`);
    }
    });


    //Update question
    const updateMutation = useMutation({
    mutationFn: (payload: CreateQuestionDto) => updateQuestion(Number(questionId), payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['questions', quizId] });
      queryClient.invalidateQueries({ queryKey: ['question', questionId] });
      navigate(`/quizzes/${quizId}/questions`);
    }
  });


  //Add new answer to form
  const handleAddAnswer = () => {
    setAnswers([...answers, { text: '', isCorrect: false }]);
  };

  //Remove answer from form
  const handleRemoveAnswer = (indexToRemove: number) => {
    setAnswers(answers.filter((_, index) => index !== indexToRemove));
  };

  //Handle answer field change
  const handleAnswerChange = (index: number, field: keyof CreateAnswerDto, value: string | boolean) => {
    const updated = [...answers];
    updated[index] = { ...updated[index], [field]: value };
    setAnswers(updated);
  };

  //Form submit handler
  const handleSubmit = (e) => {
    e.preventDefault();
    setValidationError('');

    if (!text.trim()) return setValidationError('Question content must be provided.');
    if (answers.length < 2) return setValidationError('Question must have at least two answers.');
    if (answers.some(a => !a.text.trim())) return setValidationError('All added answers must have content.');
    if (!answers.some(a => a.isCorrect)) return setValidationError('You must mark at least one correct answer.');

    const payload: CreateQuestionDto = { text, points, answers };

    if (isEditMode) {
      updateMutation.mutate(payload);
    } else {
      createMutation.mutate(payload);
    }
  };

  if (isFetchError) return <Alert severity="error">Could not fetch question data.</Alert>;

  const isSaving = createMutation.isPending || updateMutation.isPending;
  const saveError = createMutation.error || updateMutation.error;

    return (
      <Box sx={{ maxWidth: 800, mx: 'auto', mb: 10 }}>
      <AdminPageHeader 
        title={isEditMode ? 'Edit Question' : 'Add New Question'} 
       
      />

      {isFetching ? (
        <CircularProgress sx={{ display: 'block', margin: '40px auto' }} />
      ) : (
        <Paper 
          component="form" 
          onSubmit={handleSubmit} 
          elevation={2}
          sx={{ p: 4, borderRadius: 2 }}
        >
          {validationError && 
          <Alert severity="warning" sx={{ mb: 3 }}>
            {validationError}
          </Alert>
          }
          {saveError && 
          <Alert severity="error" sx={{ mb: 3 }}>
            {(saveError as Error).message}
          </Alert>
          }

          <AdminInput 
            label="Question content" 
            value={text} 
            onChange={(e) => setText(e.target.value)} 
            disabled={isSaving} 
            multiline rows={2} 
          />
          <AdminInput 
            label="Points for correct answer" 
            type="number" 
            value={points} 
            onChange={(e) => {
            const value = Number(e.target.value);
              if (value >= 1) {
                setPoints(value);
              }
            }} 
            disabled={isSaving} 
          />

          <Box 
            sx={{ 
              mt: 5, 
              mb: 3, 
              display: 'flex', 
              justifyContent: 'space-between', 
              alignItems: 'center' 
              }}
            >
            <Typography variant="h6">Answers</Typography>
            <AdminButton 
              type="button" 
              variant="outlined" 
              onClick={handleAddAnswer} 
              disabled={isSaving} 
              sx={{ width: 'auto', mt: 0, mb: 0 }}
            >
              Add answer
            </AdminButton>
          </Box>

          {answers.map((answer, index) => (
            <Paper 
            key={index} 
            variant="outlined" 
            sx={{ 
              display: 'flex', 
              alignItems: 'center', 
              p: 2, 
              mb: 2, 
              gap: 2
              }}
            >
              <AdminInput 
                label={`Option ${index + 1}`} 
                value={answer.text} 
                onChange={(e) => handleAnswerChange(index, 'text', e.target.value)} 
                disabled={isSaving} 
                sx={{ mt: 0, mb: 0 }} 
              />
              <FormControlLabel 
                control={<Checkbox checked={answer.isCorrect} 
                onChange={(e) => handleAnswerChange(index, 'isCorrect', e.target.checked)} 
                disabled={isSaving} 
                />} 
                label="Correct" sx={{ whiteSpace: 'nowrap', m: 0 }} 
              />
              <IconButton 
                color="error" 
                onClick={() => handleRemoveAnswer(index)} 
                disabled={isSaving || answers.length <= 2}
              >
                <DeleteIcon />
              </IconButton>
            </Paper>
          ))}

          <Box sx={{ 
              display: 'flex', 
              gap: 2, 
              justifyContent: 'flex-end', 
              mt: 4 
            }}
          >
            <AdminButton 
              type="button" 
              variant="outlined" 
              color="inherit" 
              onClick={() => navigate(`/quizzes/${quizId}/questions`)} 
              disabled={isSaving} 
              sx={{ width: 'auto' }}
            >
              Cancel
            </AdminButton>
            <AdminButton 
              type="submit" 
              disabled={isSaving} 
              sx={{ width: 'auto' }}
            >
              {isSaving ? 'Saving...' : 'Save question'}
            </AdminButton>
          </Box>
        </Paper>
      )}
    </Box>
    )
}