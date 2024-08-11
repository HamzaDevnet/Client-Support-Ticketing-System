import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register, RegisterResponse } from './register';
import { Userdata } from './userdata';


@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient) { }

  postRegister(userdata:Userdata): Observable<RegisterResponse> {
    const URL = 'https://localhost:7109/api/register';
    return this.http.post<RegisterResponse>(URL,userdata ); //return response token
  }
}