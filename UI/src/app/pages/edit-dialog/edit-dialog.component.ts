import { Component, Inject,  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Users } from 'app/users';
import { UsersService } from 'app/users.service';



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
  ) 
  { 
    this.editForm = this.fb.group({
      firstName: [data.firstName],
      lastName: [data.lastName],
      email: [data.email],
      mobileNumber: [data.mobileNumber],
      dateOfBirth: [data.dateOfBirth],
      address: [data.address] ,
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    const updatedData: Users = {
      userId: this.editForm.value.userId,
      firstName: this.editForm.value.firstName,
      lastName: this.editForm.value.lastName,
      userName: this.editForm.value.userName,
      email: this.editForm.value.email,
      password: this.editForm.value.password,
      mobileNumber: this.editForm.value.mobileNumber,
      dateOfBirth: this.editForm.value.dateOfBirth,
      address: this.editForm.value.address,
      userImage: this.editForm.value.userImage,
      fullName: '',
      strDateOfBirth: ''
    };
  
    console.log(this.data.userId, updatedData);
    this.UsersService.editClient(this.data.userId ,updatedData).subscribe({
      next: (response) => {
        this.dialogRef.close(response.data);
        
      },
      error: (error) => {
        console.error('Error updating client:', error);
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
