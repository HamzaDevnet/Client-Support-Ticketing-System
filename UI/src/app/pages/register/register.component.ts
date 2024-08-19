import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from 'app/register.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  backendErrors: any = {};

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private RegisterService: RegisterService,
    private ToastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      MobileNumber: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      DateOfBirth: [''],
      address: [''],
      userImage: [null], 
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const userData = this.registerForm.value;

      this.RegisterService.postRegister(userData).subscribe({
        next: (response) => {
          if (response.code === 200) {
            this.router.navigate(['/login']);
          } else {
            this.handleBackendErrors(response.message);
          }
        },
        error: (error) => {
          this.handleBackendErrors(error.error.message);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }

  handleBackendErrors(messages: any): void {
    this.backendErrors = {};
    if (Array.isArray(messages)) {
      messages.forEach((message: any) => {
        if (message.field && message.text) {
          this.backendErrors[message.field] = message.text;
        }
      });
    } else if (typeof messages === 'object') {
     
      Object.keys(messages).forEach((key) => {
        this.backendErrors[key] = messages[key];
      });
    } else {
      
      this.ToastrService.error(messages);
    }
  }

  upload(event: Event): void {
    const reader = new FileReader();
    const target = event.target as HTMLInputElement;
    const files = target.files;

    if (files && files.length > 0) {
      const file = files[0];

      reader.readAsDataURL(file);
      reader.onload = () => {
        this.registerForm.controls.userImage.setValue({
          file: reader.result.toString().split('base64,')[1],
          fileName: file.name
        });
      };
    }
  }
}