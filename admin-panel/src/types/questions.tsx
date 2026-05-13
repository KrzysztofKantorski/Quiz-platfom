import type { AnswerListItemDto, CreateAnswerDto } from "./answers";


//Get all questions from quiz
export interface QuestionListItemDto {
  id: number;
  text: string;
  points: number;
  quizId: number;
  answers: AnswerListItemDto[];
}

//Create and update question
export interface CreateQuestionDto {
  text: string;
  points: number;
  answers: CreateAnswerDto[];
}

