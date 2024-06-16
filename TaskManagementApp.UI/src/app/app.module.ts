import { RouterModule, Routes } from '@angular/router'; // Import RouterModule
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';  // Import FormsModule
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AuthenticationService } from './services/authentication.service';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/authentication/login/login.component';
import { DashboardComponent } from './components/dashboard.component';
import { SignupComponent } from './components/authentication/signup/signup.component';

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'signup', component: SignupComponent },
    { path: 'dashboard/:username', component: DashboardComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' }
  ];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,  // Add FormsModule to the imports array
    HttpClientModule,
    RouterModule.forRoot(routes) // Add RouterModule with empty routes array
  ],
  exports: [RouterModule],
  providers: [AuthenticationService],
  bootstrap: [AppComponent]
})
export class AppModule { }

