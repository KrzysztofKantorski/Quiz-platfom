import type { JSX } from "react/jsx-runtime";

//Quiz fetching
export interface QuizListItemDto {
  map(arg0: (quiz: any) => JSX.Element): import("react").ReactNode;
  id: number;
  title: string;
  description: string;
}

//Quiz creation and update
export interface QuizPayloadDto{
  title: string;
  description: string;
}

