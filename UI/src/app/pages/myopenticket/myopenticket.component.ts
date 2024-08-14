import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TicketService, Comment, CreateCommentDTO } from 'app/ticket.service';
import { Ticket } from 'app/ticket';
import { TicketStatus } from '../../enums/ticket.enum';
import { SheardServiceService } from 'app/sheard-service.service';

@Component({
  selector: 'app-myopenticket',
  templateUrl: './myopenticket.component.html',
  styleUrls: ['./myopenticket.component.scss']
})
export class MyopenticketComponent implements OnInit {
  tickets: Ticket[] = [];
  displayedColumns: string[] = ['product'];
  dataSource = new MatTableDataSource<Ticket>(this.tickets);
  selectedFilter: string = 'Status';
  ccomments: Comment[] = [];
  newComment: string = '';
  statuses : TicketStatus[]; 
  selectedTicket: Ticket | undefined;
  userId: string | undefined;

  constructor(private ticketService: TicketService, private sheardService: SheardServiceService) {
   
    this.statuses = [ 
      TicketStatus.Assigned ,
      TicketStatus.InProgress , 
      TicketStatus.Closed 
    ]
    console.log(this.statuses);
  }

  ngOnInit(): void {
    const userClaims = this.sheardService.getUserClaims();
    if (userClaims && userClaims.UserId) {
      this.userId = userClaims.UserId;
      this.getTickets(this.userId);
    } else {
      console.error('User ID not found in token');
      this.getTickets(); // Call without userId if not found
    }
  }

  getTickets(userId?: string): void {
    this.ticketService.getTickets(userId).subscribe({
      next: (tickets) => {
        this.tickets = tickets;
        this.dataSource.data = tickets;
      },
      error: (error) => {
        console.error('Error fetching tickets', error);
      },
    });
  }

  getStatusText(status: TicketStatus): string {
    switch (status) {
      case TicketStatus.Assigned:
        return 'Assigned';
      case TicketStatus.InProgress:
        return 'In Progress';
      case TicketStatus.Closed:
        return 'Closed';
      default:
        return 'Unknown';
    }
  }

  selectTicket(ticket: Ticket): void {
    this.selectedTicket = ticket;
    this.loadComments(ticket.ticketId);
  }

  loadComments(ticketId: string): void {
    this.ticketService.getComments(ticketId).subscribe({
      next: (comments) => {
        this.ccomments = comments;
      },
      error: (error) => {
        console.error('Error loading comments', error);
      },
    });
  }

  updateStatus(status: TicketStatus): void {
    if (this.selectedTicket) {
      this.selectedTicket.status = status;
      this.ticketService.updateTicketStatus(this.selectedTicket.ticketId, status).subscribe({
        next: () => {
          console.log('Ticket status updated successfully');
        },
        error: (error) => {
          console.error('Error updating ticket status', error);
        },
      });
    }
  }

  setFilter(filter: string) {
    this.selectedFilter = filter;
  }

  postComment() {
    if (this.newComment.trim() && this.selectedTicket) {
      const newComment: CreateCommentDTO  = {
        ticketId: this.selectedTicket.ticketId,
        content: this.newComment,
        userId : "db79c212-b2af-43d3-b13c-854dfc9b9c6a",
      };
      this.ticketService.addComment(newComment).subscribe({
        next: (comment) => {
          this.ccomments.push(comment);
          this.newComment = '';
        },
        error: (error) => {
          console.error('Error posting comment', error);
        },
      });
    }
  }
}
