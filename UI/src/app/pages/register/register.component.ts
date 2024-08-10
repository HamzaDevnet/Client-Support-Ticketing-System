import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from '../../register.service'; 
import { Register } from '../../register';

@Component({
  selector: 'app-register',
  templateUrl: '../register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router) { }

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
      const registerData: Register = this.registerForm.value as Register;
      this.registerService.postRegister(registerData).subscribe(
        response => {
          console.log('Registration successful', response);
          this.router.navigate(['/login']);
        },
        error => {
          console.error('Registration failed', error);
        }
      );
    } else {
      console.log('Form is invalid');
    }
  }
}