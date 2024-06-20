export interface User {
    id: string;
    email: string;
    username: string;
    avatarUrl?: string;
  }
  
  export interface AuthResponse {
    accessToken: string;
    refreshToken: string;
    user: User;
  }
  