import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'app/auth.service';
import { LoginService } from 'app/login.service';
import { UserLocalStorageService } from 'app/user-local-storage.service';
import { Userdata } from 'app/userdata';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, 
    private router: Router,
     private authService: AuthService, 
    private loginService: LoginService ,
    private userlocalstorage: UserLocalStorageService, 
  )
     { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      usernameORemail: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }//modify the names

  // onSubmit(): void {
  //   if (this.loginForm.valid) {
  //     console.log(this.loginForm.value);
  //     this.loginService.Login(this.loginForm.value).subscribe({
  //       next:(response)=>{
  //         //what response from login? 
  //         const userdata:Userdata = {
  //           //response.firstname etc..
  //           firstName: "Alanoud",
  //           lastName: "Abdullah",
  //           username:"alanoudtw", 
  //           email: "alanoud@gmail.com",
  //           phone: "987654"
  //         }
  //         this.authService.setUserData(userdata);
  //         localStorage.setItem("token","Alanoud");//token 
  //         this.router.navigate(['/dashboard']);
  //       },
  //       error:(error)=>{
  //         console.error('Error', error);
  //       }

  //     });
  //     // Perform your login logic here
    
  //   } else {
  //     console.log('Form is invalid');
  //   }
  // }

  onSubmit(): void {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
      this.loginService.Login(this.loginForm.value).subscribe({
        next: (response) => {
          // Assuming response contains a token and user data
          const token = "";
          const user = "";
  
          const userdata:Userdata = {
            //response.firstname etc..
            firstName: "Alanoud",
            lastName: "Abdullah",
            username:"alanoudtw", 
            email: "alanoud@gmail.com",
            phone: "987654"
          }
  
          this.userlocalstorage.setUserData(userdata);
          this.userlocalstorage.setUserToken(token);
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Error', error);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }
}