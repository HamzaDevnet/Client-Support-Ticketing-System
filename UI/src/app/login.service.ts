import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Login, LoginData } from './login';
import { environment } from 'environments/environment';
import { User, WebResponse } from './ticket.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {


  constructor(private http: HttpClient) {

   }

  getLogin(loginData:Login): Observable<WebResponse<LoginData>>{
    return this.http.post<WebResponse<LoginData>>((`${environment.BaseURL}/login`), loginData);
  }
}
