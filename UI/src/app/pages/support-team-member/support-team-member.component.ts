import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { SupportTeam } from 'app/support-team';
import { SupportTeamService } from 'app/support-team.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { AddTicketComponent } from '../add-ticket/add-ticket.component';

@Component({
  selector: 'app-support-team-member',
  moduleId: module.id,
  templateUrl: './support-team-member.component.html',
})
export class SupportTeamMemberComponent implements OnInit {
  supportTeam: SupportTeam[] = [];
  displayedColumns: string[] = ['firstName', 'lastName', 'username', 'MobileNumber', 'email','password', 'DateOfBirth' ,'edit'];
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


  openEditSupportDialog(supportteam: SupportTeam): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      width: '600px', 
      data: {
        firstname: supportteam.firstName,
        lastname: supportteam.lastName,
        username: supportteam.username,
        mobilephone: supportteam.MobileNumber,
        email: supportteam.email ,
        password: supportteam.password ,
        dataofbirth: supportteam.DateOfBirth, 

      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        supportteam.firstName = result.firstname;
        supportteam.lastName = result.lastName;
        supportteam.username = result.username;
        supportteam.MobileNumber = result.mobilephone;
        supportteam.email = result.email;
        supportteam.password = result.password ;
        supportteam.DateOfBirth = result.dataofbirth ;
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

  // openAddSupportDialog():void{
  //   const dialogRef = this.dialog.open(AddSupportMember, {
  //     width: '400px'
  //   });
  // }
}