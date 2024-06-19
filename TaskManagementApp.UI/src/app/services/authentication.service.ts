import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { AuthResponse } from '../models/user.model';
import { environment } from '../../environments/environment';
import { LoginUserRequest } from '../models/loginrequest.model';
import { RegisterUserRequest } from '../models/registerrequest.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private baseUrl = environment.apiUrl + '/authentication';
  private tokenKey = 'jwt_token';
  private userKey = 'current_user';

  constructor(private http: HttpClient) {}

  login(loginUserRequest: LoginUserRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, loginUserRequest).pipe(
        tap(response => {
          if (response && response.token) {
            localStorage.setItem(this.tokenKey, response.token);
            localStorage.setItem(this.userKey, JSON.stringify(response.user));
          }
        }),
        catchError(this.handleError<AuthResponse>('login'))
      );
  }

  getUsername(email: string): Observable<string> {
    return this.http.get(`${this.baseUrl}/getusername/${email}`, { responseType: 'text' }).pipe(
        map(response => response as string),
        catchError(this.handleError<string>('getUsername'))
      );
  }

  register(registerUserRequest: RegisterUserRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/register`, registerUserRequest).pipe(
      tap(response => {
        if (response && response.token) {
          localStorage.setItem(this.tokenKey, response.token);
          localStorage.setItem(this.userKey, JSON.stringify(response.user));
        }
      }),
      catchError(this.handleError<AuthResponse>('register'))
    );
  }

  getToken(): string {
    const token = localStorage.getItem(this.tokenKey);
    return token ? token : "";
  }

  getCurrentUser(): any {
    const user = localStorage.getItem(this.userKey);
    return user ? JSON.parse(user) : null;
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      if (error.status === 409) {
        return throwError(() => new Error(error.error.message));
      }
      return of(result as T);
    };
  }  
}
