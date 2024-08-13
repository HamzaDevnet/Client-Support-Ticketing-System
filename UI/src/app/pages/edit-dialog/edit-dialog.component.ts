import { Component, Inject,  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Users } from 'app/users';



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
    @Inject(MAT_DIALOG_DATA) public data: Users
  ) { 
    this.editForm = this.fb.group({
      firstName: [data.firstName],
      lastName: [data.lastName],
      username: [data.userName],
      email: [data.email],
      password: [data.password],
      mobileNumber: [data.mobileNumber],
      dateOfBirth: [data.dateOfBirth],
      address: [data.address] ,
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    const updatedData = {
      firstName: this.editForm.value.firstName,
      lastName: this.editForm.value.lastName,
      username: this.editForm.value.username ,
      email: this.editForm.value.email,
      password: this.editForm.value.password,
      mobileNumber: this.editForm.value.mobileNumber,
      dateOfBirth: this.editForm.value.dateOfBirth,
      address:this.editForm.value.address ,
    };
    this.dialogRef.close(updatedData);
  }

  deletesupport():void {
    
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
