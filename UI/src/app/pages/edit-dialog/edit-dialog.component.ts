import { Component, Inject,  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';


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
    @Inject(MAT_DIALOG_DATA) public data: {name: string, email: string, username: string, address: any}
  ) { 
    this.editForm = this.fb.group({
      name: [data.name],
      username: [data.username],
      email: [data.email],
      street: [data.address.street],
      city: [data.address.city],
      state: [data.address.state],
      zipcode: [data.address.zipcode]
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    const updatedData = {
      name: this.editForm.value.name,
      username: this.editForm.value.username,
      email: this.editForm.value.email,
      address: {
        street: this.editForm.value.street,
        city: this.editForm.value.city,
        state: this.editForm.value.state,
        zipcode: this.editForm.value.zipcode
      }
    };
    this.dialogRef.close(updatedData);
  }

}
