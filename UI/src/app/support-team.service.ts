import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Users } from './users';
import { environment } from 'environments/environment';
import { WebResponse } from './pages/web-response';

@Injectable({
  providedIn: 'root',
})
export class SupportTeamService {
  private usersUrl = 'https://localhost:7125/api/Users';

  constructor(private http: HttpClient) {}

  getSupportTeam(): Observable<Users[]> {
    return this.http.get<WebResponse<Users[]>>(`${this.usersUrl}/support-team-members`).pipe(
      map(response => response.data)
    );
  }


  addSupportMember(data: Users): Observable<WebResponse<Users>> {
    return this.http.post<WebResponse<Users>>(`${environment.BaseURL2}/RegisterSupportTeamMember`, data)
  }

  deleteSupportMember(userid:number):Observable<WebResponse<boolean>> {
    return this.http.delete<WebResponse<boolean>>(`${environment.BaseURL}/users/${userid}`);
  }
}