import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from 'app/register.service';
import { UserLocalStorageService } from 'app/user-local-storage.service'; // Assuming this import is needed
import { Userdata } from 'app/userdata'; // Adjust the path as needed

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
    private userLocalStorage: UserLocalStorageService
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
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const userData: Userdata = this.registerForm.value;
      console.log('Form data:', userData);

      this.RegisterService.postRegister(userData).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          this.userLocalStorage.setUserData(userData); 
          //token
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Registration failed:', error);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }
}