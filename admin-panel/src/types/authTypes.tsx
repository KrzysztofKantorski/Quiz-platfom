export interface LoginDto {
  email: string;
  password: string;
}

export interface LoginResponse {
  message: string; 
}

export interface UserProfileDto {
  id: number;
  email: string;
  role: string;
}