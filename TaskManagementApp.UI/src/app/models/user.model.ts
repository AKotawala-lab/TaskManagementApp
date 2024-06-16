export interface User {
    id: string;
    email: string;
    username: string;
    avatarUrl?: string;
  }
  
  export interface AuthResponse {
    token: string;
    user: User;
  }
  