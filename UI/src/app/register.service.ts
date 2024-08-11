import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register } from './register';
import { Userdata } from './userdata';


@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient) { }

  postRegister(userdata:Userdata): Observable<Register> {
    const URL = 'https://localhost:7109/api/register';
    return this.http.post<Register>(URL,userdata ); //return response token
  }
}