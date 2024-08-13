import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { SupportTeam } from 'app/support-team';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { SupportTeamService } from 'app/support-team.service';
import { AddclientticketComponent } from '../addclientticket/addclientticket.component';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';
import { UserLocalStorageService } from 'app/user-local-storage.service';

@Component({
  selector: 'app-clienttickets',
  templateUrl: './clienttickets.component.html',
  styleUrls: ['./clienttickets.component.scss']
})
export class ClientticketsComponent implements OnInit {
  Tickets: Ticket[] = [];
  displayedColumns: string[] = ['ticketId', 'product', 'createdDate', 'status', 'action'];
  dataSource = new MatTableDataSource<Ticket>(this.Tickets);


  constructor(public dialog: MatDialog, private ticketService : TicketService , private UserLocalStorageService : UserLocalStorageService){}

  ngOnInit(): void {
    this.getTickets();
  }

  getTickets():void{
    this.ticketService.getTickets().subscribe({
      next:(Tickets)=> {
        this.Tickets =Tickets ;
      },
      error: (error) => {
        console.error('Error listing Tickets', error);
      }
    })
  }

  addTicket(): void {
    this.UserLocalStorageService.getCurrentUser();
    const dialogRef = this.dialog.open(AddclientticketComponent, {
      width: '400px'
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Ticket Data:', result);
      }
    });
  }

  openVewiDialog(ticket: Ticket): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      width: '600px',
      data: {
        ticketId: ticket.ticketId ,
        product:ticket.product , 
        status: ticket.status ,
        createDate: ticket.createdDate ,
        assignedToUserName: ticket.assignedToUserName ,
        problemDescription :ticket.problemDescription 

      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
      ticket.ticketId = result.ticketId ,
      ticket.product = result.product ,
      ticket.status = result.status, 
      ticket.createdDate = result.createDate ,
      ticket.assignedToUserName = result.assignedToUserName
      ticket.problemDescription = result.problemDescription , 
        this.dataSource.data = [...this.Tickets];
      }
    });
  }
}


