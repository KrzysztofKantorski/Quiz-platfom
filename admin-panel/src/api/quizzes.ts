import { fetchClient } from './fetchCilent';
import type { QuizListItemDto, QuizPayloadDto } from '../types/quiz';

//Get all quizzes
export const getQuizzes = (): Promise<QuizListItemDto[]> => {
  return fetchClient<QuizListItemDto[]>('/admin/quizzes'); 
};

//Delete quiz
export const deleteQuiz = (id: number): Promise<void> => {
  return fetchClient(`/admin/quizzes/${id}`, { method: 'DELETE' });
};

//Get quiz by id
export const getQuizById = (id: number): Promise<QuizListItemDto> => {
  return fetchClient(`/admin/quizzes/${id}`);
};

//Create new quiz
export const createQuiz = (data: QuizPayloadDto) => {
  return fetchClient('/admin/quizzes', { method: 'POST', body: JSON.stringify(data) });
};

//Update quiz by id
export const updateQuiz = (params: { id: number; data: QuizPayloadDto }) => {
  return fetchClient(`/admin/quizzes/${params.id}`, { method: 'PUT', body: JSON.stringify(params.data) });
};