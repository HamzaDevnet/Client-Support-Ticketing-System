import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Ticket } from './ticket';
import { environment } from 'environments/environment';
import { SheardServiceService } from './sheard-service.service';

export interface User {
  userId: string;
  fullName: string;
}//if it for members put in member interface 

export interface WebResponse<T> {
  data: T;
  code: number;
  message: string;
} //seperate file because it's shareable 

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private baseUrl = 'https://localhost:7125/api/Tickets';
  private usersUrl = 'https://localhost:7125/api/Users';

  constructor(private http: HttpClient, private SheardServiceService: SheardServiceService) {}

  getTickets(): Observable<Ticket[]> {
    const headers = this.SheardServiceService.Header_Get();
    return this.http.get<Ticket[]>(`${environment.BaseURL}/Tickets`, { headers });
  }

  getTicketById(id: string): Observable<Ticket> {
    const headers = this.SheardServiceService.Header_Get();
    return this.http.get<Ticket>(`${this.baseUrl}/${id}`);
  }

  assignTicket(ticketId: string, assignedTo: string): Observable<void> {
    const headers = this.SheardServiceService.Header_Post();
    return this.http.put<void>(`${this.baseUrl}/Assign`, { ticketId, assignedTo }, { headers });
  }

  closeTicket(ticketId: string): Observable<void> {
    const headers = this.SheardServiceService.Header_Post();
    return this.http.put<void>(`${this.baseUrl}/Close/${ticketId}`, { }, { headers });
  }

  getSupportTeamMembers(): Observable<User[]> {
    const headers = this.SheardServiceService.Header_Get();
    return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/support-team-members`).pipe(
      map(response => response.data)
    );
  } //take it 

  getClients(): Observable<User[]> {
    const headers = this.SheardServiceService.Header_Get();
    return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/external-clients`).pipe(
      map(response => response.data)
    );
  }
}
