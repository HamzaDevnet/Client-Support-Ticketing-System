import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { SupportTeam } from 'app/support-team';
import { SupportTeamService } from 'app/support-team.service';

@Component({
  selector: 'app-add-ticket',
  templateUrl: './add-ticket.component.html',
  styleUrls: ['./add-ticket.component.scss']
})
export class AddTicketComponent implements OnInit {
  AssignedTo: FormGroup;
  TicketName: FormGroup;
  supportTeam: SupportTeam[] = [];

  constructor(
    private supportTeamService: SupportTeamService,
    private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<AddTicketComponent>,
  ) {
    this.AssignedTo = this._formBuilder.group({
      firstCtrl: [''],
    });
    this.TicketName = this._formBuilder.group({
      userCtrl: [''],
    });
  }

  ngOnInit(): void {
    this.getSupportTeam();
  }

  getSupportTeam(): void {
    this.supportTeamService.getSupportTeam().subscribe({
      next: (supportTeam) => {
        this.supportTeam = supportTeam;
      },
      error: (error) => {
        console.error('Error fetching support teams information', error);
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    const updatedData = {
      name: this.AssignedTo.value.name,
    };
    this.dialogRef.close(updatedData);
  }
}