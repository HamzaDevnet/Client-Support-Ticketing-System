import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { RegisterData } from './register';
import { Userdata } from './userdata';
import { User, WebResponse } from './ticket.service';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient) { }

  postRegister(registerdata:Userdata): Observable<WebResponse<RegisterData>> {
    return this.http.post<WebResponse<RegisterData>>((`${environment.BaseURL}/RegisterClient`),registerdata);
  }
  postRegisterSupport(registerdata:Userdata): Observable<WebResponse<RegisterData>> {
    return this.http.post<WebResponse<RegisterData>>((`${environment.BaseURL}/teammember/register`),registerdata);
  }

}

// getSupportTeamMembers(): Observable<User[]> {
//   return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/support-team-members`).pipe(
//     map(response => response.data)
//   );
// }


// getLogin(loginData:Login): Observable<LoginResponse> {
//   return this.http.post<LoginResponse>((`${environment.BaseURL}/login`), loginData);
// }