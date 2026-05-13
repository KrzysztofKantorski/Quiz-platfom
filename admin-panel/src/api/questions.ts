import type { CreateQuestionDto, QuestionListItemDto } from '../types/questions';
import { fetchClient } from './fetchCilent';


//Get all questions for quiz
export const getQuestionsForQuiz = (quizId: number): Promise<QuestionListItemDto[]> => {
  return fetchClient<QuestionListItemDto[]>(`/admin/quizzes/${quizId}/questions`);
}

//Get question by id
export const getQuestionById = (id: number):Promise<QuestionListItemDto> => {
  return fetchClient(`/admin/questions/${id}`);
};

//Delete question
export const deleteQuestion = (id: number): Promise<void> => {
  return fetchClient(`/admin/questions/${id}`, { method: 'DELETE' });
};


// Update question by id 
export const updateQuestion = (id: number, data: CreateQuestionDto): Promise<any> => {
  return fetchClient(`/admin/questions/${id}`, { 
    method: 'PUT', 
    body: JSON.stringify(data) 
  });
};

// Add question to quiz
export const addQuestion = (quizId: number, data: CreateQuestionDto): Promise<any> => {
  return fetchClient(`/admin/quizzes/${quizId}/questions`, { 
    method: 'POST', 
    body: JSON.stringify(data) 
  });
};


