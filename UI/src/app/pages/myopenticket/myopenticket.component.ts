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
  comments: Comment[] = []; // Initialize as an empty array
  newComment: string = '';
  statuses: TicketStatus[];
  selectedTicket: Ticket | undefined;
  userId: string;

  constructor(private ticketService: TicketService, private sheardService: SheardServiceService) {
    this.statuses = [
      TicketStatus.Assigned,
      TicketStatus.InProgress,
      TicketStatus.Closed
    ];
    console.log(this.statuses);
  }

  ngOnInit(): void {
    this.userId = this.sheardService.getUserId();
    if (this.userId) {
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
        this.comments = comments;
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
    // Ensure comments is initialized as an empty array if it's not already
    this.comments = this.comments || [];

    if (this.newComment.trim() && this.selectedTicket) {
      const newComment: CreateCommentDTO = {
        ticketId: this.selectedTicket.ticketId,
        content: this.newComment,
        userId: this.userId, // Use the userId from the service
      };

      this.ticketService.addComment(newComment).subscribe({
        next: (comment) => {
          this.comments.push(comment); // Push the new comment to the array
          this.newComment = '';
        },
        error: (error) => {
          console.error('Error posting comment', error);
        },
      });
    } else {
      console.error('New comment text or selected ticket is missing');
    }
  }

  getUserDisplayName(comment: Comment): string {
    const userName = comment.userName ? comment.userName : 'Anonymous';
    const role = this.getUserRole(comment.userType);
    return `${userName} (${role})`;
  }

  getUserRole(userType: string | null): string {
    switch (userType) {
      case '0':
        return 'Client';
      case '1':
        return 'Support Team Member';
      case '2':
        return 'Manager';
      default:
        return 'User';
    }
  }
}
