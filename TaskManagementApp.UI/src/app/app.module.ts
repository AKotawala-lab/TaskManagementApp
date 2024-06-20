import { RouterModule, Routes } from '@angular/router'; // Import RouterModule
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';  // Import FormsModule
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgbModule  } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from './services/authentication.service';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/authentication/login/login.component';
import { DashboardComponent } from './components/dashboard.component';
import { SignupComponent } from './components/authentication/signup/signup.component';
import { AddEditTaskComponent } from './components/task/add-edit-task.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'signup', component: SignupComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' }
  ];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    DashboardComponent,
    AddEditTaskComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,  // Add FormsModule to the imports array
    ReactiveFormsModule,
    HttpClientModule,
    NgbModule,
    RouterModule.forRoot(routes) // Add RouterModule with empty routes array
  ],
  exports: [RouterModule],
  providers: [
    AuthenticationService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

