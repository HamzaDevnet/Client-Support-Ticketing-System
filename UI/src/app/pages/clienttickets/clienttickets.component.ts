import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TicketService, Comment, CreateCommentDTO } from 'app/ticket.service';
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
  userId: string | undefined;

  constructor(public dialog: MatDialog, private ticketService: TicketService, private sheardService: SheardServiceService) {}

  ngOnInit(): void {
    const userClaims = this.sheardService.getUserClaims();
    if (userClaims && userClaims.UserId) {
      this.userId = userClaims.UserId;
      this.getTickets(this.userId);
    } else {
      console.error('User ID not found in token');
    }
  }

  getTickets(userId: string): void {
    this.ticketService.getTickets(userId).subscribe({
      next: (tickets) => {
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
        this.getTickets(this.userId); // Refresh the ticket list after adding a new ticket
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
      }
    });
  }

  postComment() {
    if (this.newComment.trim() && this.selectedTicket) {
      const newComment: CreateCommentDTO = {
        ticketId: this.selectedTicket.ticketId,
        content: this.newComment , 
        userId : ""
      };
      this.ticketService.addComment(newComment).subscribe({
        next: (comment) => {
          this.comments.push(comment);
          this.newComment = '';
        },
        error: (error) => {
          console.error('Error posting comment', error);
        }
      });
    }
  }
}
