import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from 'app/register.service';
import { UserLocalStorageService } from 'app/user-local-storage.service'; 
import { Userdata } from 'app/userdata'; 
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private RegisterService: RegisterService,
    private ToastrService : ToastrService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      MobileNumber: ['', [Validators.pattern('^[0-9]+$')]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      DateOfBirth: [''],
      address:[''],
      userImage:[null], 
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const userData: Userdata = this.registerForm.value;
      console.log('Form data:', userData);

      this.RegisterService.postRegister(userData).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          this.router.navigate(['/login']);
          if(response.code === 200){
          this.ToastrService.success("Registration successful");
          }
        },
        error: (error) => {
          console.error('Registration failed:', error);
          this.ToastrService.error(error.message);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }

  upload(event: Event){
    const reader = new FileReader();
    const target = event.target as HTMLInputElement;
    const files = target.files;

    if (files && files.length > 0) {
      const file = files[0];

      reader.readAsDataURL(file);
      reader.onload = () => {
        console.log(reader.result);
        console.log(file.name);
        this.registerForm.controls.userImage.setValue({
          file : reader.result.toString().split('base64,')[1] ,
          fileName: file.name
        })
      };

    }
  }
  
}

