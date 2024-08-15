import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TicketService, Comment, CreateCommentDTO, CreateTicketDTO } from 'app/ticket.service';
import { Ticket } from 'app/ticket';
import { MatDialog } from '@angular/material/dialog';
import { AddclientticketComponent } from '../addclientticket/addclientticket.component';
import { SheardServiceService } from 'app/sheard-service.service';

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
  userId: string;

  constructor(public dialog: MatDialog, private ticketService: TicketService, private sheardService: SheardServiceService) {}

  ngOnInit(): void {
    this.userId = this.sheardService.getUserId();
    if (!this.userId) {
      console.error('User ID not found in token, using default user ID');
      this.userId = 'eaf994d3-0c78-4288-b45a-5b3b3de88421'; // Default user ID
    }
    this.getTickets(this.userId);
  }

  getTickets(userId: string): void {
    this.ticketService.getTickets(userId).subscribe({
      next: (tickets) => {
        console.log('Tickets received:', tickets); // Add this line for debugging
        this.tickets = tickets;
        this.dataSource.data = tickets;
      },
      error: (error) => {
        console.error('Error listing tickets', error);
      }
    });
  }

  addTicket(): void {
    const dialogRef = this.dialog.open(AddclientticketComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const newTicket: CreateTicketDTO = {
          product: result.product,
          problemDescription: result.problemDescription,
        };
        this.ticketService.addTicket(newTicket).subscribe({
          next: (ticket) => {
            this.tickets.push(ticket);
            this.dataSource.data = [...this.tickets];
          },
          error: (error) => {
            console.error('Error adding ticket', error);
          }
        });
      }
    });
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

  postComment() {
    if (this.newComment.trim() && this.selectedTicket) {
      const newComment: CreateCommentDTO = {
        ticketId: this.selectedTicket.ticketId,
        content: this.newComment,
        userId: this.userId, // Use userId from the component, which defaults to the provided ID if not found in the token
      };

      this.ticketService.addComment(newComment).subscribe({
        next: (comment) => {
          this.comments.push(comment);
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
