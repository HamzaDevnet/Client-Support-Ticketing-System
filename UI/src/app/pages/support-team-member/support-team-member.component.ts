import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { SupportTeamService } from 'app/support-team.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { Users } from 'app/users';
import { AddSupportComponent } from '../add-support/add-support.component';
import { UsersService } from 'app/users.service';
import { UserStatus } from 'app/enums/user.enum';




@Component({
  selector: 'app-support-team-member',
  moduleId: module.id,
  templateUrl: './support-team-member.component.html',
})
export class SupportTeamMemberComponent implements OnInit {
  supportTeam: Users[] = [];
  UserStatus = UserStatus;
  displayedColumns: string[] = ['firstName', 'lastName', 'mobileNumber', 'email', 'ActivateDeactivate' , 'edit'];
  dataSource = new MatTableDataSource<Users>(this.supportTeam);

  constructor(private supportTeamService: SupportTeamService, public dialog: MatDialog ,private UsersService : UsersService ) {}

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
      data: supportteam 
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
       this.getSupportTeam();
      }
    });
  }

  openAddSupportDialog(): void {
    const dialogRef = this.dialog.open(AddSupportComponent, {
      width: '500px'
    });
    
    
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      console.log('The dialog was closed');
      if (result && result.isdone) {
        this.getSupportTeam();
      }
    });
  }

  deactivateSupport(support:Users):void{
    this.UsersService.deactivatecUser(support.userId).subscribe({
      next: (response)=>{
        if(response.data === true){
          this.getSupportTeam();
        }

      }
    })
  }

  activateSupport(support:Users):void{
    this.UsersService.activateUser(support.userId).subscribe({
      next: (response)=>{
        if(response.data === true){
          this.getSupportTeam();
        }

      }
    })
  }

}