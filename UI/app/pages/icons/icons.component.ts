import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { SupportTeam } from 'app/support-team';
import { SupportTeamService } from 'app/support-team.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { AddTicketComponent } from '../add-ticket/add-ticket.component';

@Component({
  selector: 'icons-cmp',
  moduleId: module.id,
  templateUrl: './icons.component.html',
})
export class IconsComponent implements OnInit {
  supportTeam: SupportTeam[] = [];
  displayedColumns: string[] = ['id', 'name', 'username', 'email', 'edit'];
  dataSource = new MatTableDataSource<SupportTeam>(this.supportTeam);

  constructor(private supportTeamService: SupportTeamService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.getSupportTeam();
  }

  getSupportTeam(): void {
    this.supportTeamService.getSupportTeam().subscribe({
      next: (supportTeam) => {
        this.supportTeam = supportTeam;
        this.dataSource.data = supportTeam;
      },
      error: (error) => {
        console.error('Error fetching support teams information', error);
      },
    });
  }

  openEditDialog(supportteam: SupportTeam): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      width: '600px', // Adjust the width as needed
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

  openAddTicketDialog(): void {
    const dialogRef = this.dialog.open(AddTicketComponent, {
      width: '400px'
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      // Handle result if needed
    });
  }
}