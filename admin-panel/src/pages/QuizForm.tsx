import { useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { getQuizById, updateQuiz } from '../api/quizzes';
import { AdminPageHeader } from '../components/AdminPageHeader';
import { useEffect, useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createQuiz } from '../api/quizzes';
import { useNavigate } from 'react-router-dom';
import { Alert, Box, CircularProgress, Paper } from '@mui/material';
import { AdminInput } from '../components/AdminInput';
import { AdminButton } from '../components/AdminButton';
export const QuizForm = () => {

    const navigate = useNavigate();
    const queryClient = useQueryClient();

    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [validationError, setValidationError] = useState('');


    //Get id from url
    const { id } = useParams(); 

    //Check if user wants to update or create new quiz
    const isEditMode = Boolean(id);

    //Featch quiz data if in edit mode
    const { data: quizData, isLoading: isFetching, isError: isFetchError } = useQuery({
        queryKey: ['quiz', id],
        queryFn: () => getQuizById(Number(id)), 
        enabled: isEditMode 
    });

    //Fill form fields with quiz data when fetched
    useEffect(() => {
        if (quizData) {
            setTitle(quizData.title);
            setDescription(quizData.description);
        }
    }, [quizData]);

    //Save new quiz
    const createMutation = useMutation({
        mutationFn: createQuiz,
        onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ['quizzes'] });
        navigate('/quizzes'); // Wracamy do listy po sukcesie
        }
    });


    //Update quiz data
    const updateMutation = useMutation({
        mutationFn: updateQuiz,
        onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ['quizzes'] });

        queryClient.invalidateQueries({ queryKey: ['quiz', id] }); 
        navigate('/quizzes');
        }
    });

    //Login states and error handling
    const handleSubmit = (e) => {
        e.preventDefault();
        setValidationError('');

        //Validation
        if (!title.trim() || !description.trim()) {
            setValidationError('Title and description are required.');
            return;
        }
        else if (title.length > 50) {
            setValidationError('Title cannot be longer than 50 characters.');
            return;
        }
        else if (description.length < 5 || description.length > 100) {
            setValidationError('Description must be between 5 and 100 characters.');
            return;
        }

        const payload = { title, description };

        if (isEditMode) {
        updateMutation.mutate({ id: Number(id), data: payload });
        } else {
        createMutation.mutate(payload);
        }
    };

    if(isFetchError){
        return (
            <Alert severity="error" sx={{ mb: 3 }}>
                Could not fetch quiz data. It may not exist.
            </Alert>
        )
    }


    const isSaving = createMutation.isPending || updateMutation.isPending;
    const saveError = createMutation.error || updateMutation.error
    return (
        <div>
            <Box 
                component="section" 
                sx={{ 
                    mx: 'auto',
                    mt: 4, 
                    mb: 4, 
                    display: 'flex', 
                    flexDirection: 'column', 
                    alignItems: 'center' 
                }}
            >

            <AdminPageHeader 
                title={isEditMode ? `Edit quiz` : 'Create new quiz'} 
            />
        
            {isFetching ? 
                (
                    <CircularProgress sx={{ display: 'block', margin: '40px auto' }} />
                )
                :
                (      
                    <Paper elevation={2} sx={{ p: 4, borderRadius: 2, maxWidth: 600, display: "flex",  alignItems: "center",}}>
                    <Box component="form" onSubmit={handleSubmit}>
            
                        {
                        validationError && 
                            <Alert severity="warning" sx={{ mb: 2 }}>
                                {validationError}
                            </Alert>
                        }

                        {
                        saveError && 
                            <Alert severity="error" sx={{ mb: 2 }}>
                                {(saveError as Error).message}
                            </Alert>
                        }

                        <AdminInput
                        label="Quiz title"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        disabled={isSaving}
                        />

                        <AdminInput
                        label="Description"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        disabled={isSaving}
                        multiline
                        rows={4}
                        />

                        <AdminButton type="submit" disabled={isSaving}>
                        {isSaving ? 'Saving...' : 'Save quiz'}
                        </AdminButton>

                        <AdminButton 
                        variant="outlined" 
                        color="inherit" 
                        onClick={() => navigate('/quizzes')}
                        disabled={isSaving}
                        sx={{ mt: 1, mb: 0 }}
                        >
                        Cancel
                        </AdminButton>

                    </Box>
                    </Paper>
                )
            }
        
        </Box>
        </div>
)}