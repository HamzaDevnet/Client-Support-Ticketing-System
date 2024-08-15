import { Routes } from '@angular/router';

import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import { LandingComponent } from './pages/landing/landing.component';
import { TicketDetailsComponent } from './pages/ticket-details/ticket-details.component';
import { ClienttickectdetailsComponent } from './pages/clienttickectdetails/clienttickectdetails.component';
import { ForgetComponent } from './pages/forget/forget.component'; // Import ForgetComponent

export const AppRoutes: Routes = [
  {
    path: '',
    redirectTo: 'landing',
    pathMatch: 'full',
  },
  {
    path: 'landing',
    component: LandingComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'forgot-password', // Add this route for Forgot Password
    component: ForgetComponent
  },
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./layouts/admin-layout/admin-layout.module').then(x => x.AdminLayoutModule)
      },
      {
        path: 'userprofile',
        component: UserProfileComponent
      },
      {
        path: 'ticketdetails',
        component: ClienttickectdetailsComponent
      },
    ],
  },
];