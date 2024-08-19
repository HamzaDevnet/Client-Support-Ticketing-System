import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TicketService, Comment, CreateCommentDTO, CreateTicketDTO } from 'app/ticket.service';
import { Ticket } from 'app/ticket';
import { MatDialog } from '@angular/material/dialog';
import { AddclientticketComponent } from '../addclientticket/addclientticket.component';
import { SheardServiceService } from 'app/sheard-service.service';
import { UserType } from 'app/enums/user.enum';  // Import the UserType enum
import { environment } from 'environments/environment';

@Component({
  selector: 'app-clienttickets',
  templateUrl: './clienttickets.component.html',
  styleUrls: ['./clienttickets.component.scss']
})
export class ClientticketsComponent implements OnInit {
  tickets: Ticket[] = [];
  displayedColumns: string[] = ['ticketId', 'product', 'createdDate', 'status', 'action'];
  dataSource = new MatTableDataSource<Ticket>(this.tickets);
  selectedTicket: Ticket | undefined;
  comments: Comment[] = [];
  newComment: string = '';
  userId: string | undefined;
  isLoadingTickets = false;
  isLoadingComments = false;
  backendURL = environment.BackendURL;

  constructor(public dialog: MatDialog, private ticketService: TicketService, private sheardService: SheardServiceService) {}

  ngOnInit(): void {
    this.userId = this.sheardService.getUserId();
    if (!this.userId) {
      console.error('User ID not found in token');
      return; // Stop execution if the userId is not found
    }
    this.getTickets(this.userId);
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
        console.error('Error listing tickets', error);
        this.isLoadingTickets = false;
      }
    });
  }

  addTicket(): void {
    const dialogRef = this.dialog.open(AddclientticketComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.isdone) {
        this.getTickets(this.userId)
      }
    });
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

  postComment() {
    if (this.newComment.trim() && this.selectedTicket) {
      const newComment: CreateCommentDTO = {
        ticketId: this.selectedTicket.ticketId,
        content: this.newComment,
        userId: this.userId!, // Using non-null assertion since userId should exist
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
    const role = this.getUserRole(comment.userType); 
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

  getStatusText(status: number): string {
    switch (status) {
      case 0:
        return 'Open';
      case 1:
        return 'In Progress';
      case 2:
        return 'Closed';
      default:
        return 'New';
    }
  }
}
