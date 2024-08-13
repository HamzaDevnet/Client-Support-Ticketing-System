import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Ticket } from './ticket';
import { environment } from 'environments/environment';

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

  constructor(private http: HttpClient) {}

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${environment.BaseURL}/Tickets/tickets`);
  }

  getTicketById(id: string): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.baseUrl}/${id}`);
  }

  assignTicket(ticketId: string, assignedTo: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Assign`, { ticketId, assignedTo });
  }

  getSupportTeamMembers(): Observable<User[]> {
    return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/support-team-members`).pipe(
      map(response => response.data)
    );
  } //take it 

  getClients(): Observable<User[]> {
    return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/external-clients`).pipe(
      map(response => response.data)
    );
  }
}
