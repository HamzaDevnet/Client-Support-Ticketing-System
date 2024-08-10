import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Login } from './login';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  Login(data:Login): Observable<Login> {
    const URL = 'https://localhost:7109/api/login';
    return this.http.post<Login>(URL,data);
  }
}
