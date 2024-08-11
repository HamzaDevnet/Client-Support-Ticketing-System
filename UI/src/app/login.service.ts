import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Login, LoginResponse } from './login';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  getLogin(loginData:Login): Observable<LoginResponse> {
    const URL = 'https://localhost:7109/api/login';
    return this.http.post<LoginResponse>(URL, loginData);
  }
}