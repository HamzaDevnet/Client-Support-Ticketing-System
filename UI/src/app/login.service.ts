import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SheardServiceService } from './sheard-service.service';
import { Observable } from 'rxjs';
import { Login, LoginResponse } from './login';

@Injectable({
  providedIn: 'root'
})
export class LoginService {


  constructor(private http: HttpClient, private SheardServiceService: SheardServiceService) {

   }

  getLogin(loginData:Login): Observable<LoginResponse> {
    const URL = this.SheardServiceService.getApiUrl() + '/login';
    return this.http.post<LoginResponse>(URL, loginData);
  }
}