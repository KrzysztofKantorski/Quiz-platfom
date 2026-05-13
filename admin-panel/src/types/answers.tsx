import type { JSX } from "react/jsx-runtime";

//Get all answers from question
export interface AnswerListItemDto {
  map(arg0: (answer: any) => JSX.Element): import("react").ReactNode;
  id: number;
  text: string;
  isCorrect: boolean;
}

export interface CreateAnswerDto {
  id?: number; 
  text: string;
  isCorrect: boolean;
}


