import { fetchClient } from './fetchCilent';
import type { LoginDto, LoginResponse, UserProfileDto } from "../types/authTypes";


//Login admin
export const loginAdmin = (data: LoginDto): Promise<LoginResponse> => {
  return fetchClient<LoginResponse>('/auth/login', {
    method: 'POST',
    body: JSON.stringify(data),
  });
};

//Logout admin
export const logoutAdmin = ()  => {
  return fetchClient('/auth/logout', {
    method: 'POST'
  });
};


//Get user profile
export const getMe = (): Promise<UserProfileDto> => {
  return fetchClient('/auth/me', { method: 'GET' });
};