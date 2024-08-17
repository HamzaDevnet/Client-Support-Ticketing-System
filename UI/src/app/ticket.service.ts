import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Ticket } from './ticket';
import { environment } from 'environments/environment';
import { SheardServiceService } from './sheard-service.service';
import { TicketStatus } from './enums/ticket.enum';
import { UserType } from './enums/user.enum';

export interface User {
  userId: string;
  fullName: string;
}

export interface WebResponse<T> {
  data: T;
  code: number;
  message: string;
}
export interface CreateTicketDTO {
  product: string;
  problemDescription: string;
}

export interface Comment {
  commentId: string;
  content: string;
  createdDate: Date;
  fullName: string | null;
  userType: UserType;
  userId: string;
  ticketId: string;
}

export interface CreateCommentDTO {
  ticketId: string;
  content: string;
  userId : string ;
}

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private usersUrl = 'https://localhost:7125/api/Users';
  private baseUrl = `${environment.BaseURL}/Tickets`;
  private commentsUrl = `${environment.BaseURL}/Comments`;

  constructor(private http: HttpClient, private sheardService: SheardServiceService) {}

  getTickets(userId?: string): Observable<Ticket[]> {
    const headers = this.sheardService.getToken();
    const url = userId ? `${this.baseUrl}?userId=${userId}` : this.baseUrl;
    return this.http.get<{ result: { data: Ticket[] } }>(url, { headers }).pipe(
      map(response => response.result.data)
    );
  }

 

  getTicketById(id: string): Observable<Ticket> {
    const headers = this.sheardService.Header_Get();
    return this.http.get<{ code: number, data: Ticket }>(`${this.baseUrl}/${id}`, { headers }).pipe(
      map(response => {
        console.log('API response:', response);
        if (response && response.data) {
          return response.data;
        } else {
          throw new Error('Invalid response format');
        }
      })
    );
  }

  assignTicket(ticketId: string, assignedTo: string): Observable<void> {
    const headers = this.sheardService.Header_Post();
    return this.http.put<void>(`${this.baseUrl}/Assign`, { ticketId, assignedTo }, { headers });
  }

  closeTicket(ticketId: string): Observable<void> {
    const headers = this.sheardService.Header_Post();
    return this.http.put<void>(`${this.baseUrl}/Close/${ticketId}`, {}, { headers });
  }

  getSupportTeamMembers(): Observable<User[]> {
    const headers = this.sheardService.Header_Get();
    return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/support-team-members`, { headers })
      .pipe(map(response => response.data));
  }

  getClients(): Observable<User[]> {
    const headers = this.sheardService.Header_Get();
    return this.http.get<WebResponse<User[]>>(`${this.usersUrl}/external-clients`, { headers })
      .pipe(map(response => response.data));
  }

  getTicketsCountByState(): Observable<Ticket[]> {
    const headers = this.sheardService.getToken();
    return this.http.get<Ticket[]>(`${environment.BaseURL}/Dashboard/TicketsCountByState`, { headers })
  }

  updateTicketStatus(ticketId: string, status: TicketStatus): Observable<void> {
    const headers = this.sheardService.Header_Post();
    return this.http.put<void>(`${this.baseUrl}/${ticketId}`, { status }, { headers });
  }

  RemoveTicketStatus(ticketId: string, status: TicketStatus): Observable<void> {
    const headers = this.sheardService.Header_Post();
    return this.http.put<void>(`${this.baseUrl}/Remove/${ticketId}`, { status }, { headers });
  }

  getComments(ticketId: string, pageNumber: number = 1, pageSize: number = 100): Observable<Comment[]> {
    const headers = this.sheardService.Header_Get();
    const url = `${this.commentsUrl}?ticketId=${ticketId}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get<{ data: Comment[] }>(url, { headers }).pipe(
      map(response => response.data)
    );
  }

  addComment(comment: CreateCommentDTO): Observable<Comment> {
    const headers = this.sheardService.Header_Post();
    return this.http.post<{ data: Comment }>(this.commentsUrl, comment, { headers }).pipe(
      map(response => response.data)
    );
  }

  addTicket(ticket: CreateTicketDTO): Observable<Ticket> {
    const headers = this.sheardService.Header_Post();
    return this.http.post<{ data: Ticket }>(this.baseUrl, ticket, { headers }).pipe(
      map(response => response.data)
    );
  }
}
