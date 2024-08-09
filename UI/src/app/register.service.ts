import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register } from './register';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private apiUrl = 'https://localhost:7109/api/register';  // Adjust URL as necessary
  
  constructor(private http: HttpClient) { }

  // Method to register a new user
  postRegister(dto: Register): Observable<Register> {
    return this.http.post<Register>(this.apiUrl, dto);
  }
  
}
