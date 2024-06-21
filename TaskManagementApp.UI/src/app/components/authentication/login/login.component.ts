import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { LoginUserRequest } from '../../../models/loginrequest.model';
import { HealthCheckService } from '../../../services/healthcheck.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string = "";
  username: string = "";
  password: string = "";
  errorMessage: string = "";
  showPasswordCard: boolean = false;
  fieldTextType: boolean = false;

  isDatabaseReady = false;
  motivationalQuotes = [
    "The way to get started is to quit talking and begin doing.",
    "The only limit to our realization of tomorrow is our doubts of today.",
    "It's not the load that breaks you down, it's the way you carry it."
  ];
  currentQuote = '';
  interval: any;

  constructor(
    private router: Router,
    private healthCheckService: HealthCheckService,
    private authService: AuthenticationService) {}

  ngOnInit(): void {
    this.startQuoteRotation();
    this.healthCheckService.checkHealth().subscribe(isHealthy => {
      this.isDatabaseReady = isHealthy;
      if (isHealthy) {
        clearInterval(this.interval);
      }
    });
  }

  startQuoteRotation(): void {
    let index = 0;
    this.currentQuote = this.motivationalQuotes[index];
    this.interval = setInterval(() => {
      index = (index + 1) % this.motivationalQuotes.length;
      this.currentQuote = this.motivationalQuotes[index];
    }, 7000);
  }

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
