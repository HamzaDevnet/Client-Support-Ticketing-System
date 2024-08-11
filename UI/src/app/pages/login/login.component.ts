import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'app/login.service';
import { Userdata } from 'app/userdata';
import { UserLocalStorageService } from 'app/user-local-storage.service'; // Assuming this import is needed
import { Register } from 'app/register';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loginService: LoginService, 
    private userLocalStorage: UserLocalStorageService 
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      EmailOrUserName: ['', [Validators.required]],
      password: ['', Validators.required]
    });
  }
  onSubmit(): void {
    if (this.loginForm.valid) {
      console.log('Form data:', this.loginForm.value);
      this.loginService.getLogin(this.loginForm.value).subscribe({
        next: (response) => { 
          console.log('API response:', response);
          const token = response.token;
          const user = response.user;

          const userdata: Userdata = {
            firstName: user.firstName,
            lastName: user.lastName,
            username: user.username,
            email: user.email,
            MobileNumber: user.MobileNumber,
            password: user.password,
            DateOfBirth: user.DateOfBirth
          };

          this.userLocalStorage.setUserData(userdata); 
          this.userLocalStorage.setToken(token); 
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Error logging in', error);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }
}