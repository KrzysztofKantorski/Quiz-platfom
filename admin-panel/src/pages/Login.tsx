import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Paper, Alert, CircularProgress } from '@mui/material';
import { getMe, loginAdmin } from '../api/auth';
import { AdminInput } from '../components/AdminInput';
import { AdminButton } from '../components/AdminButton';
import { useMutation } from '@tanstack/react-query';
import type { LoginDto } from '../types/authTypes';

export const Login = () => {

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const [validationError, setValidationError] = useState('');
  const navigate = useNavigate();


  const loginMutation = useMutation({
    mutationFn: async (credentials: LoginDto) => {
      // 1. Wysyłamy prośbę o logowanie (Backend ustawia ciasteczko HttpOnly)
      await loginAdmin(credentials);
      
      // 2. Skoro mamy już ciasteczko, od razu pytamy serwer "Kim ja jestem?"
      const userProfile = await getMe();

      // 3. Weryfikacja roli na żywo!
      if (userProfile.role !== 'Admin') {
        // Opcjonalnie: Możesz tu wywołać też API do wylogowania (żeby usunąć to ciasteczko z przeglądarki)
        // await logoutAdmin(); 
        
        throw new Error('Access denied');
      }

      return userProfile; // Zwracamy dane, gdyby były potrzebne w onSuccess
    },
    onSuccess: () => {
      localStorage.setItem('isAuthenticated', 'true');
      navigate('/');
    },
    //Clear password field
    onError: () => {
      setPassword(''); 
    }
  });


  const handleSubmit = async (e ) => {
    e.preventDefault(); 
    setValidationError(''); 

    //Clean errors
    loginMutation.reset();  

    //Validation
    if (!email || !password) {
      setValidationError('Fields cannot be empty.');
      return;
    }
      loginMutation.mutate({ email, password });
    }




  return (

    <Box 
      sx={{ 
          height: '100vh', 
          display: 'flex', 
          alignItems: 'center', 
          justifyContent: 'center', 
          bgcolor: 'background.default' 
        }}
      >
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 400 }}>

       <Typography 
          variant="h3" 
          component="h5" 
          sx={{textAlign: "center"}}
        >
          Admin Panel
        </Typography>

        {validationError && (
          <Alert severity="warning">{validationError}</Alert>
        )}

        {loginMutation.isError && (
        <Alert severity="error">
          {(loginMutation.error as Error).message || 'Incorrect email or password.'}
        </Alert>
        )}


        <form onSubmit={handleSubmit}>
          <AdminInput
            label="Adres E-mail"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            disabled={loginMutation.isPending}
          />
          
          <AdminInput
            label="Hasło"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            disabled={loginMutation.isPending}
          />
          
          <AdminButton 
            type="submit" 
            disabled={loginMutation.isPending}
            startIcon={loginMutation.isPending ? <CircularProgress size={20} color="inherit" /> : null}
          >
          {loginMutation.isPending ? 'Logowanie...' : 'Zaloguj się'}
          </AdminButton>
        </form>
      </Paper>
    </Box>
  );
};