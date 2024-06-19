import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { LoginUserRequest } from '../../../models/loginrequest.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = "";
  username: string = "";
  password: string = "";
  errorMessage: string = "";
  jwToken: string = "";
  showPasswordCard: boolean = false;
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

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  onSubmit() {
    if (!this.email) {
      return;
    }

    this.authService.getUsername(this.email).subscribe({
      next: (uname) => {
        if (uname) {
          this.username = uname;
          this.showPasswordCard = true;
          this.hideLoginCard();
        } else {
          this.errorMessage = "User not found";
        }
      },
      error: () => {
        this.errorMessage = "User not found";
      }
    });
  }

  hideLoginCard() {
    const loginCard = document.getElementById('login-card');
    if (loginCard) {
      loginCard.style.animation = 'slide-out 0.5s forwards';
      setTimeout(() => {
        loginCard.style.display = 'none';
        const passwordCard = document.getElementById('password-card');
        if (passwordCard) {
          passwordCard.classList.add('show');
        }
      }, 500);
    }
  }

  onPasswordSubmit() {
    if (!this.password) {
      return;
    }

    const loginData: LoginUserRequest = {
      username: this.username,
      password: this.password
    };

    this.authService.login(loginData).subscribe({
      next: (response) => {
        if (response) {
          this.jwToken = response.token;

          this.router.navigate(['/dashboard']);

        } else {
          this.errorMessage = 'Invalid password. Please try again.';
        }
      },
      error: () => {
        this.errorMessage = 'Invalid password. Please try again.';
      }
    });
  }

  onCreateAccountClick() {
    this.router.navigate(['/signup']);
  }
}
