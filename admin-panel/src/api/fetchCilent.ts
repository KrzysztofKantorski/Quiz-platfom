const BASE_URL = 'http://localhost:5216/api';

export const fetchClient = async <T>(endpoint: string, options: RequestInit = {}): Promise<T> => {
  const headers: HeadersInit = {
    'Content-Type': 'application/json',
    ...options.headers,
  };

  const response = await fetch(`${BASE_URL}${endpoint}`, {
    ...options,
    headers,
    
    //Send httponly cookie
    credentials: 'include',
  });

  //Remove flag from localstorage if token expired
  if(response.status === 401) {
    localStorage.removeItem('isAuthenticated');

    //Navigate user to login page
    window.location.href = '/login';
    throw new Error('Unauthorized. Please log in again.');
  }
   if(response.status === 403) {
    window.location.href = '/login';
    throw new Error('Forbidden. You do not have permission to access this resource.');
  }

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    throw new Error(errorData?.message || `HTTP Error: ${response.status}`);
  }

  if (response.status === 204) {
    return {} as T;
  }


 

  return await response.json();
};