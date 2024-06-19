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
  private accessTokenKey = 'jwt_access_token';
  private refreshTokenKey = 'jwt_refresh_token';
  private userKey = 'current_user';

  constructor(private http: HttpClient) {}

  login(loginUserRequest: LoginUserRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, loginUserRequest).pipe(
        tap(response => {
          if (response) {
            this.storeTokens(response);
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
        if (response) {
          this.storeTokens(response);
        }
      }),
      catchError(this.handleError<AuthResponse>('register'))
    );
  }

  getToken(): string {
    const aToken = localStorage.getItem(this.accessTokenKey);
    return aToken ? aToken : "";
  }

  getRefreshToken(): string {
    const rToken = localStorage.getItem(this.refreshTokenKey);
    return rToken ? rToken : "";
  }

  storeTokens(authResponse: AuthResponse): void {
    localStorage.setItem(this.accessTokenKey, authResponse.accessToken);
    localStorage.setItem(this.refreshTokenKey, authResponse.refreshToken);
    localStorage.setItem(this.userKey, JSON.stringify(authResponse.user));
  }

  refreshToken(): Observable<AuthResponse> {
    const refreshRequest = {
      accessToken: this.getToken(),
      refreshToken: this.getRefreshToken()
    };
    return this.http.post<AuthResponse>(`${this.baseUrl}/refresh`, refreshRequest).pipe(
      tap(authResponse => this.storeTokens(authResponse)),
      catchError(this.handleError<AuthResponse>('refreshToken'))
    );
  }

  getCurrentUser(): any {
    const user = localStorage.getItem(this.userKey);
    return user ? JSON.parse(user) : null;
  }

  logout(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
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
