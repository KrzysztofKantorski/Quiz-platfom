import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AdminLayout } from './layouts/AdminLayout';
import { Dashboard } from './pages/Dashboard';
import { QuizList } from './pages/QuizList';
import { QuizForm } from './pages/QuizForm';
import { QuestionList } from './pages/QuestionList';
import { Login } from './pages/Login';
import {ProtectedRoute} from './layouts/ProtectedRoute';
import { QuestionForm } from './pages/QuestionForm';
const router = createBrowserRouter([
  //Public routes
  {
    path: '/login',
    element: <Login />,
  },
  //Protected routes
  {
    element: <ProtectedRoute />, 
    children: 
      [
        {
          path: '/',
            element: <AdminLayout />, 
            children: [
              {
                index: true,
                element: <Dashboard />,
              },
              {
                path: 'quizzes',
                element: <QuizList />,
              },
              {
                path: 'quizzes/:id/edit',
                element: <QuizForm />,
              },
              {
                path: 'quizzes/new',
                element: <QuizForm />,
              },
              {
                path: 'quizzes/new',
                element: <QuizForm />,
              },
              {
                path: '/quizzes/:id/questions',
                element: <QuestionList />,
              },
              {
                path: "/quizzes/:id/questions/:questionId/edit",
                element: <QuestionForm />
              },
              {
                path: "/quizzes/:id/questions/new",
                element: <QuestionForm />
              },
            ],
        }
      ],
  },
]);

export const App = () => {
  return <RouterProvider router={router} />;
};