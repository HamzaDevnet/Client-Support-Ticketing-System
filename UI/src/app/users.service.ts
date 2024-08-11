import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Users } from './users';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient) { }

  private supportMember ='https://localhost:7125/api/Users/support-team-members';
  private clients = 'https://localhost:7125/api/Users/external-clients';

getSupport(): Observable<Users[]> {
  return this.http.get<Users[]>(this.supportMember);
}

getClients(): Observable<Users[]> {
  return this.http.get<Users[]>(this.clients);
}
  // getSupportTeamMembers(): Observable<User[]> {
  //   return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/support-team-members`).pipe(
  //     map(response => response.data)
  //   );
  // }

  // getClients(): Observable<User[]> {
  //   return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/external-clients`).pipe(
  //     map(response => response.data)
  //   );
  // }

}