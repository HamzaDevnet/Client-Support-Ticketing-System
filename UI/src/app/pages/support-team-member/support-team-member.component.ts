import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { SupportTeamService } from 'app/support-team.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { AddTicketComponent } from '../add-ticket/add-ticket.component';
import { Users } from 'app/users';
import { AddSupportComponent } from '../add-support/add-support.component';




@Component({
  selector: 'app-support-team-member',
  moduleId: module.id,
  templateUrl: './support-team-member.component.html',
})
export class SupportTeamMemberComponent implements OnInit {
  supportTeam: Users[] = [];
  displayedColumns: string[] = ['firstName', 'lastName', 'username', 'mobileNumber', 'email', 'dateOfBirth' ,'edit'];
  dataSource = new MatTableDataSource<Users>(this.supportTeam);

  constructor(private supportTeamService: SupportTeamService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.getSupportTeam();
  }

  getSupportTeam(): void {
    this.supportTeamService.getSupportTeam().subscribe({
      next: (supportTeam) => {
        this.supportTeam = supportTeam;
        this.dataSource.data = supportTeam;
        console.log(supportTeam);
      },
      error: (error) => {
        console.error('Error fetching support teams information', error);
      },
    });
  }


  openEditSupportDialog(supportteam: Users): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      width: '600px',
      data: {
        firstName: supportteam.firstName,
        lastName: supportteam.lastName,
        username: supportteam.userName,
        mobileNumber: supportteam.mobileNumber,
        email: supportteam.email,
        password: supportteam.password,
        dateOfBirth: supportteam.dateOfBirth,
        address: supportteam.address
      }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        supportteam.firstName = result.firstName;
        supportteam.lastName = result.lastName;
        supportteam.userName = result.username;
        supportteam.mobileNumber = result.mobileNumber;
        supportteam.email = result.email;
        supportteam.password = result.password;
        supportteam.dateOfBirth = result.dateOfBirth;
        supportteam.address = result.address;
        this.dataSource.data = [...this.supportTeam];
      }
    });
  }

  openAddSupportDialog(): void {
    const dialogRef = this.dialog.open(AddSupportComponent, {
      width: '500px'
    });
    
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result && result.isdone) {
        this.getSupportTeam();
      }
    });
  }

}