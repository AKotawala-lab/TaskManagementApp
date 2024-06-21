import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, timer } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HealthCheckService {
  private baseUrl = environment.apiUrl + '/healthcheck';

  constructor(private http: HttpClient) {}

  checkHealth(): Observable<boolean> {
    return timer(0, 5000).pipe(
      switchMap(() => this.http.get<{ status: string }>(this.baseUrl)),
      switchMap(response => of(response.status === 'Healthy')),
      catchError(() => of(false))
    );
  }
}
