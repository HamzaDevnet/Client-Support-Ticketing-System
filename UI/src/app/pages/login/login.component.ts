import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'app/login.service';
import { UserLocalStorageService } from 'app/user-local-storage.service';
import { UserType } from 'app/enums/user.enum';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  errorMessage: string;

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
          if(response.code == 200){
            console.log('API response:', response);
            const token = response.data.token;
            this.userLocalStorage.setToken(token);
            this.navigate(response.data.userType);
          } else {
            alert(response.message);
          }
        },
        error: (error) => {
          console.error('Error logging in', error);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }

  private navigate(userType: UserType): void {
    switch(userType) {
      case UserType.Client:
        this.router.navigate(['/myTickets']);
        break;
      case UserType.Support:
        this.router.navigate(['/openticket']);
        break;
      case UserType.Manager:
        this.router.navigate(['/dashboard']);
        break;
    }
  }
}
