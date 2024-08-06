import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';
import { AddTicketComponent } from '../add-ticket/add-ticket.component';


@Component({
    moduleId: module.id,
    selector: 'app-ticket',
    templateUrl: 'ticket.component.html'
})

export class TicketComponent {
    tickets:Ticket[]=[];
    displayedColumns: string[] = ['id', 'title', 'description','AssignedTo'];
    dataSource = new MatTableDataSource<Ticket>(this.tickets);
    selectedFilter: string = 'Filter Tickets';

    constructor(private ticketService: TicketService, public dialog: MatDialog){}

    ngOnInit(): void {
        this.getTickets();
      }

      getTickets(): void {
        this.ticketService.getTickets().subscribe({
          next: (tickets) => {
            this.tickets = tickets;
            this.dataSource.data = tickets;
          },
          error: (error) => {
            console.error('Error fetching tickets', error);
          },
        });
      }

      allTickets():void{
        this.getTickets();
        console.log("All tickets succesfully display");
      }

      filterTickets(filter: string): void {
        this.selectedFilter = filter;
    
        if (filter === 'All Tickets') {
          this.getTickets();
        }
    }

    openAssignedDialog(): void {
        const dialogRef = this.dialog.open(AddTicketComponent, {
          width: '400px'
        });
        dialogRef.afterClosed().subscribe(result => {
          console.log('The dialog was closed');
          // Handle result if needed
        });
      }
      
    
}
