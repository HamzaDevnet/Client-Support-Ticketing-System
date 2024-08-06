import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';


@Component({
    moduleId: module.id,
    selector: 'maps-cmp',
    templateUrl: 'maps.component.html'
})

export class MapsComponent {
    tickets:Ticket[]=[];
    displayedColumns: string[] = ['id', 'title', 'description'];
    dataSource = new MatTableDataSource<Ticket>(this.tickets);

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
      
    
}
