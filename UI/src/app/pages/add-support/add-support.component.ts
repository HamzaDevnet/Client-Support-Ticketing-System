import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Users } from 'app/users';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { SupportTeamService } from 'app/support-team.service';

@Component({
  selector: 'app-add-support',
  templateUrl: './add-support.component.html',
  styleUrls: ['./add-support.component.scss']
})
export class AddSupportComponent  {
  supportForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddSupportComponent> ,
    private SupportTeamService : SupportTeamService ,
  ) {
    this.supportForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      mobileNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      dateOfBirth: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.supportForm.valid) {
      this.SupportTeamService.addSupportMember(this.supportForm.value).subscribe({
        next:(response)=>{
          this.dialogRef.close({
            isdone:true
          });

        }  
      })
      
    }
  }
}