import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register } from './register';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient) { }

  Register(data:Register): Observable<Register> {
    const URL = 'https://localhost:7109/api/register';
    return this.http.post<Register>(URL,data);
  }
}
