import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TicketService, Comment, CreateCommentDTO } from 'app/ticket.service';
import { Ticket } from 'app/ticket';
import { TicketStatus } from '../../enums/ticket.enum';
import { SheardServiceService } from 'app/sheard-service.service';
import { UserType } from 'app/enums/user.enum';
import { environment } from 'environments/environment';

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
  comments: Comment[] = [];
  newComment: string = '';
  statuses: TicketStatus[];
  selectedTicket: Ticket | undefined;
  userId: string | undefined;
  isLoadingTickets = false;
  isLoadingComments = false;
  backendURL = environment.BackendURL;


  constructor(private ticketService: TicketService, private sheardService: SheardServiceService) {
    this.statuses = [
      TicketStatus.Assigned,
      TicketStatus.InProgress,
      TicketStatus.Closed
    ];
  }

  ngOnInit(): void {
    this.userId = this.sheardService.getUserId();
    if (this.userId) {
      this.getTickets(this.userId);
    } else {
      console.error('User ID not found in token');
      return; // Stop execution if the userId is not found
    }
  }

  getTickets(userId: string): void {
    this.isLoadingTickets = true;
    this.ticketService.getTickets(userId).subscribe({
      next: (tickets) => {
        this.tickets = tickets;
        this.dataSource.data = tickets;
        this.isLoadingTickets = false;
      },
      error: (error) => {
        console.error('Error fetching tickets', error);
        this.isLoadingTickets = false;
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
    this.loadTicket(ticket.ticketId);
    this.loadComments(ticket.ticketId);
  }

  loadTicket(ticketId:string):void{
    this.ticketService.getTicketById(ticketId).subscribe({
      next:(ticket) =>{
        this.selectedTicket = ticket;
      }
    })
  }



  loadComments(ticketId: string): void {
    this.isLoadingComments = true;
    this.ticketService.getComments(ticketId).subscribe({
      next: (comments) => {
        this.comments = comments.sort((a, b) => new Date(a.createdDate).getTime() - new Date(b.createdDate).getTime());
        this.isLoadingComments = false;
      },
      error: (error) => {
        console.error('Error loading comments', error);
        this.isLoadingComments = false;
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

  postComment() {
    if (this.newComment.trim() && this.selectedTicket) {
      const newComment: CreateCommentDTO = {
        ticketId: this.selectedTicket.ticketId,
        content: this.newComment,
        userId: this.userId!, // Non-null assertion since userId should exist
      };

      this.ticketService.addComment(newComment).subscribe({
        next: (comment) => {
          this.comments.push(comment);
          this.newComment = '';
          setTimeout(() => {
            const commentInput = document.querySelector<HTMLInputElement>('.input-group input');
            commentInput?.focus();
          }, 0);
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
    const fullName = comment.fullName ? comment.fullName : 'Anonymous';
    const role = this.getUserRole(comment.userType); // Pass the userType enum value
    return `${fullName} (${role})`;
  }

  getUserRole(userType: UserType): string {
    switch (userType) {
      case UserType.Client:
        return 'Client';
      case UserType.Support:
        return 'Support Team Member';
      case UserType.Manager:
        return 'Manager';
      default:
        return 'User';
    }
  }
}
