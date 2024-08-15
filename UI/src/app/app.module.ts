import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ToastrModule } from "ngx-toastr";

import { SidebarModule } from './sidebar/sidebar.module';
import { FooterModule } from './shared/footer/footer.module';
import { NavbarModule } from './shared/navbar/navbar.module';
import { FixedPluginModule } from './shared/fixedplugin/fixedplugin.module';

import { AppComponent } from './app.component';
import { AppRoutes } from './app.routing';

import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';

import { SupportTeamService } from './support-team.service';
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { CommonModule } from "@angular/common";
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from "@angular/material/button";
import { MatDialogModule } from "@angular/material/dialog";
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { EditDialogComponent } from './pages/edit-dialog/edit-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from "@angular/forms";
import { MatMenuModule } from '@angular/material/menu';
import { AddTicketComponent } from './pages/add-ticket/add-ticket.component';
import { MatSelectModule } from "@angular/material/select";
import { UserProfileComponent } from './user-profile/user-profile.component';
import { SupportTeamMemberComponent } from './pages/support-team-member/support-team-member.component';
import { TicketComponent } from './pages/ticket/ticket.component';
import { RegisterComponent } from './pages/register/register.component';
import { MatNativeDateModule } from "@angular/material/core";
import { LoginComponent } from './pages/login/login.component';
import { MyopenticketComponent } from './pages/myopenticket/myopenticket.component';
import { FormsModule } from '@angular/forms';
import { ClientticketsComponent } from './pages/clienttickets/clienttickets.component';
import { AddclientticketComponent } from './pages/addclientticket/addclientticket.component';
import { TicketDetailsComponent } from './pages/ticket-details/ticket-details.component'; 
import { AssignTicketComponent } from './pages/assign-ticket/assign-ticket.component';
import { UsersService } from "./users.service";
import { TicketService } from "./ticket.service";
import { LandingComponent } from './pages/landing/landing.component';
import { ClientComponent } from './pages/client/client.component';
import { AddSupportComponent } from './pages/add-support/add-support.component';
import { ClienttickectdetailsComponent } from './pages/clienttickectdetails/clienttickectdetails.component';
import { DatePipe } from "@angular/common";
import { ForgetComponent } from './pages/forget/forget.component';
import {TranslateLoader, TranslateModule, TranslatePipe} from '@ngx-translate/core';
import { TranslateHttpLoader } from "@ngx-translate/http-loader";

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    SupportTeamMemberComponent,
    EditDialogComponent,
    AddTicketComponent,
    UserProfileComponent,
    TicketComponent,
    RegisterComponent,
    LoginComponent,
    MyopenticketComponent,
    ClientticketsComponent,
    AddclientticketComponent,
    TicketDetailsComponent,
    AssignTicketComponent,
    LandingComponent,
    ClientComponent,
    AddSupportComponent,
    ClienttickectdetailsComponent,
    ForgetComponent,


  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    MatCheckboxModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    FormsModule,
    MatMenuModule,
    MatCardModule,
    MatCardModule,
    MatCheckboxModule,
    MatNativeDateModule,
    RouterModule.forRoot(AppRoutes),
    SidebarModule,
    NavbarModule,
    ToastrModule.forRoot(),
    FooterModule,
    FixedPluginModule,
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      }
  })
  ],
  providers: [SupportTeamService, UsersService, TicketService, DatePipe],
  bootstrap: [AppComponent],
})
export class AppModule { }
