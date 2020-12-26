import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthentificationService {
  private url: string = environment.url;
  constructor(private http: HttpClient) {}

  getHeaders() {
    return {
      headers: new HttpHeaders({
        'content-type': 'application/json',
      }),
      withCredentials: false,
    };
  }

  isAuthenticated(): boolean {
    return localStorage.getItem('token') !== null
  }

  getToken(): string {
    return localStorage.getItem('token');
  }

  getName(): string {
    return localStorage.getItem('name');
  }

  isAdmin(): boolean {
    return localStorage.getItem('role') === 'admin';
  }

  register(data: any): Observable<any> {
    return this.http
      .post(this.url + 'user/register', data, this.getHeaders())
      .pipe(
        tap((response) => console.log(response))
      );
  }

  login(data: any): Observable<any> {
    return this.http
      .post(this.url + 'user/authenticate', data, this.getHeaders())
      .pipe(
        tap((response) => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('name', response.name);
          localStorage.setItem('role', response.role);
        })
      );
  }
}
