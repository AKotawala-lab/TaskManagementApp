import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Task } from '../models/task.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private baseUrl = environment.apiUrl + '/task';

  constructor(private http: HttpClient) { }

  getTasksForDate(date: string, token: string): Observable<Task[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Task[]>(`${this.baseUrl}/date/${date}`, { headers }).pipe(
      catchError(this.handleError<Task[]>('getTasksForDate', []))
    );
  }

  getAllTasks(userId: string, token: string): Observable<Task[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    const params = new HttpParams().set('userId', userId);
    return this.http.get<Task[]>(`${this.baseUrl}`, { headers, params }).pipe(
      catchError(this.handleError<Task[]>('getAllTasks', []))
    );
  }

  createTask(task: Task, token: string): Observable<Task> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<Task>(this.baseUrl, task, { headers }).pipe(
      catchError(this.handleError<Task>('createTask'))
    );
  }

  updateTask(taskId: string, task: Task, token: string): Observable<Task> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<Task>(`${this.baseUrl}/${taskId}`, task, { headers }).pipe(
      catchError(this.handleError<Task>('updateTask'))
    );
  }

  deleteTask(taskId: string, token: string): Observable<void> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<void>(`${this.baseUrl}/${taskId}`, { headers });
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
