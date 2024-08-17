import { Component, Inject,  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Users } from 'app/users';
import { UsersService } from 'app/users.service';
import { DatePipe } from '@angular/common';



@Component({
  selector: 'app-edit-dialog',
  templateUrl: './edit-dialog.component.html',
  styleUrls: ['./edit-dialog.component.scss']
})
export class EditDialogComponent {
  editForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Users ,
    private UsersService : UsersService,
    private datePipe : DatePipe ,
  ) 
  { 
    console.log(data);
    this.editForm = this.fb.group({
      firstName: [data.firstName],
      lastName: [data.lastName],
      email: [data.email],
      mobileNumber: [data.mobileNumber],
      dateOfBirth: [this.datePipe.transform(data.dateOfBirth,"yyyy-MM-dd")],
      address: [data.address] ,
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    console.log(this.data.userId, this.editForm.value);
    this.UsersService.editUser(this.data.userId,this.editForm.value).subscribe({
      next: (response) => {
        this.dialogRef.close({isdone:true});
        
      },
      error: (error) => {
        console.error('Error updating', error);
      }
    });
  }

 


  // onSubmit() {
  //   if (this.supportForm.valid) {
  //     this.SupportTeamService.addSupportMember(this.supportForm.value).subscribe({
  //       next:(response)=>{
  //         this.dialogRef.close({
  //           isdone:true
  //         });

  //       }  
  //     })
      
  //   }
  // }
}
