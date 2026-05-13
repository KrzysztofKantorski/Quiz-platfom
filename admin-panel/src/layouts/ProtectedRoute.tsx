import { Navigate, Outlet } from 'react-router-dom';

export const ProtectedRoute = () => {

    //Get user login flag
    const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';

    //Check if flag from localstorage exists
    if (!isAuthenticated) {

        return <Navigate to="/login" replace />;
    }

    //Flag is true
    return <Outlet />;
};
