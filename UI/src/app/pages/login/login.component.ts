import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'app/login.service';
import { Userdata } from 'app/userdata';
import { UserLocalStorageService } from 'app/user-local-storage.service'; // Assuming this import is needed


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
      username: ['', [Validators.required]],
      password: ['', Validators.required]
    });
  }

  login(): void {
    if (this.loginForm.valid) {
      console.log('Form data:', this.loginForm.value);
      this.loginService.getLogin(this.loginForm.value).subscribe({
        next: (response) => { 
          console.log('API response:', response);
          const token = response.data.token;
  
             //check for password "if statment"
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

