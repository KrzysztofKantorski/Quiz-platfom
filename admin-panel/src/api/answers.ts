import type { CreateQuestionDto, QuestionListItemDto } from '../types/questions';
import { fetchClient } from './fetchCilent';


//Get all questions for quiz
export const getQuestionsForQuiz = (quizId: number): Promise<QuestionListItemDto[]> => {
  return fetchClient<QuestionListItemDto[]>(`/admin/quizzes/${quizId}/questions`);
}

//Add answer to question
//Add question to quiz
export const updateAnswers = (params: { id: number; data: CreateQuestionDto }) => {
  return fetchClient(`/admin/quizzes/${params.id}/questions`, { method: 'POST', body: JSON.stringify(params.data) });
};

//Delete question
export const deleteQuestion = (id: number): Promise<void> => {
  return fetchClient(`/admin/questions/${id}`, { method: 'DELETE' });
};


//Update question by id
export const updateQuestion = (params: { id: number; data: CreateQuestionDto }) => {
  return fetchClient(`/admin/questions/${params.id}`, { method: 'PUT', body: JSON.stringify(params.data) });
};


//Add question to quiz
export const addQuestion = (params: { id: number; data: CreateQuestionDto }) => {
  return fetchClient(`/admin/quizzes/${params.id}/questions`, { method: 'POST', body: JSON.stringify(params.data) });
};