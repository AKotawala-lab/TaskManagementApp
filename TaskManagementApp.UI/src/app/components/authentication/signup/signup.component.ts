import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { RegisterUserRequest } from '../../../models/registerrequest.model';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  email: string = "";
  username: string = "";
  password: string = "";
  errorMessage: string = "";
  fieldTextType: boolean = false;

  constructor(
    private router: Router,
    private authService: AuthenticationService) {}

  onEmailInput() {
    this.errorMessage = '';
  }

  clearEmail() {
    this.email = '';
    this.errorMessage = '';
  }

  clearUsername() {
    this.username = '';
    this.errorMessage = '';
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  onSubmit() {
    if (!this.email || !this.username || !this.password) {
      this.errorMessage = 'All fields are required';
      return;
    }

    const registerData: RegisterUserRequest = {
      username: this.username,
      email: this.email,
      password: this.password,
      avatarUrl: ""
    };

    this.authService.register(registerData).subscribe({
      next: (response) => {
        if (response) {
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = 'Registration failed. Please try again.';
        }
      },
      error: (error) => {
        console.error('Registration error:', error);
        this.errorMessage = error.message || 'Registration failed. Please try again.';
      }
    });
  }

  onLoginClick() {
    this.router.navigate(['/login']);
  }
}
