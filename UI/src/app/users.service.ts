import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Users } from './users';
import { HttpClient } from '@angular/common/http';
import { User, WebResponse } from './ticket.service';
import { environment } from 'environments/environment';
import { SheardServiceService } from './sheard-service.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient , private SheardServiceService:SheardServiceService) { }

  private supportMember ='https://localhost:7125/api/Users/support-team-members';
  private clients = 'https://localhost:7125/api/Users/external-clients';


getClientsbyManager(): Observable<Users[]> {
  const headers = this.SheardServiceService.getToken();
  return this.http.get<WebResponse<Users[]>>(`${environment.BaseURL}/Users/clients`,{headers}).pipe(
    map(response => response.data)
  );
}

editClient(id: string , userdata:Users): Observable<WebResponse<Users>> {
  const headers = this.SheardServiceService.getToken();
  return this.http.put<WebResponse<Users>>(`${environment.BaseURL}/users/${id}`,userdata,{headers})

}

deactivateClient(id: string , userdata: Users):Observable<WebResponse<User>>{
  return this.http.patch<WebResponse<User>>(`${environment.BaseURL}/users/${id}/deactivate`, userdata);
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