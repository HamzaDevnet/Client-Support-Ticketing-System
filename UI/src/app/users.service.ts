import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Users } from './users';
import { HttpClient } from '@angular/common/http';
import { User, WebResponse } from './ticket.service';
import { environment } from 'environments/environment';
import { SheardServiceService } from './sheard-service.service';
import { Ticket } from './ticket';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient, private sheardService: SheardServiceService) {}

  private supportMemberUrl = `${environment.BaseURL}/Users/support-team-members`;
  private clientsUrl = `${environment.BaseURL}/Users/external-clients`;


  getUserInfo():Observable<WebResponse<Users>>{
    const headers = this.sheardService.getToken();
    return this.http.get<WebResponse<Users>>(`${environment.BaseURL}/users/Profile`,{headers});
  }

  getSupportTeamMembers(): Observable<User[]> {
    const headers = this.sheardService.getToken();
    return this.http.get<WebResponse<User[]>>(`${environment.BaseURL}/Users/clients`, { headers }).pipe(
      map(response => response.data)
    );
  }

  getClients(): Observable<User[]> {
    const headers = this.sheardService.getToken();
    return this.http.get<WebResponse<User[]>>(`${environment.BaseURL}/Users/clients`, { headers }).pipe(
      map(response => response.data)
    );
  }

    getTeamMemberTicketsCount():Observable<any[]> {
      const headers = this.sheardService.getToken();
      return this.http.get<any[]>(`${environment.BaseURL}/Dashboard/TeamMemberTicketsCount`, { headers })
    }

    getClientTicketsCount():Observable<any[]> {
      const headers = this.sheardService.getToken();
      return this.http.get<any[]>(`${environment.BaseURL}/Dashboard/ClientTicketsCount`, { headers })
    }

    getTeamMemberClosedTicketsCount():Observable<any[]> {
      const headers = this.sheardService.getToken();
      return this.http.get<any[]>(`${environment.BaseURL}/Dashboard/TeamMemberClosedTicketsCount`, { headers })
    }
  

  getClientsbyManager(): Observable<Users[]> {
    const headers = this.sheardService.getToken();
    return this.http.get<WebResponse<Users[]>>(`${environment.BaseURL}/Users/clients`, { headers }).pipe(
      map(response => response.data)
    );
  }

  editUser(id: string, userdata: Users): Observable<WebResponse<Users>> {
    const headers = this.sheardService.getToken();
    return this.http.put<WebResponse<Users>>(`${environment.BaseURL}/users/${id}`, userdata, { headers });
  }

  deactivatecUser(id: string): Observable<WebResponse<boolean>> {
    const headers = this.sheardService.Header_Post();
    return this.http.patch<WebResponse<boolean>>(`${environment.BaseURL}/users/${id}/deactivate`,null, {headers});
  }

  activateUser(id: string): Observable<WebResponse<boolean>> {
    const headers = this.sheardService.Header_Post();
    return this.http.patch<WebResponse<boolean>>(`${environment.BaseURL}/users/${id}/activate`, null,{headers} );
}

}
