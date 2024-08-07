import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { SupportTeam } from 'app/support-team';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { SupportTeamService } from 'app/support-team.service';
import { AddclientticketComponent } from '../addclientticket/addclientticket.component';

@Component({
  selector: 'app-clienttickets',
  templateUrl: './clienttickets.component.html',
  styleUrls: ['./clienttickets.component.scss']
})
export class ClientticketsComponent implements OnInit {
  supportTeam: SupportTeam[] = [];
  displayedColumns: string[] = ['id', 'name', 'username', 'email', 'edit'];
  dataSource = new MatTableDataSource<SupportTeam>(this.supportTeam);


  constructor(private supportTeamService: SupportTeamService, public dialog: MatDialog){}

  ngOnInit(): void {
    this.getSupportTeam();
  }

  getSupportTeam(): void {
    this.supportTeamService.getSupportTeam().subscribe({
      next: (supportTeam) => {
        this.supportTeam = supportTeam;
        this.dataSource.data = supportTeam;
        console.log('succuessful fetching support teams information');
      },
      error: (error) => {
        console.error('Error fetching support teams information', error);
      },
    });
  }

  addTicket(): void {
    const dialogRef = this.dialog.open(AddclientticketComponent, {
      width: '400px'
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Ticket Data:', result);
      }
    });
  }

  openVewiDialog(supportteam: SupportTeam): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      width: '600px',
      data: {
        name: supportteam.name,
        email: supportteam.email,
        username: supportteam.username,
        address: supportteam.address
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        supportteam.name = result.name;
        supportteam.email = result.email;
        supportteam.username = result.username;
        supportteam.address = result.address;
        this.dataSource.data = [...this.supportTeam];
      }
    });
  }
}