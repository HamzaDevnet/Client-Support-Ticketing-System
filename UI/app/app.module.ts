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
import { HttpClientModule } from "@angular/common/http";
import { CommonModule } from "@angular/common";
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from "@angular/material/button";
import { MatDialogModule } from "@angular/material/dialog";
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { IconsComponent } from "./pages/icons/icons.component";
import { EditDialogComponent } from './pages/edit-dialog/edit-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from "@angular/forms";
import { MapsComponent } from "./pages/maps/maps.component";
import {MatMenuModule} from '@angular/material/menu';
import { AddTicketComponent } from './pages/add-ticket/add-ticket.component';
import { MatSelectModule } from "@angular/material/select";


@NgModule({
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    IconsComponent,
    EditDialogComponent,
    MapsComponent,
    AddTicketComponent
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
    MatMenuModule,
    MatCardModule,

    RouterModule.forRoot(AppRoutes, {
      useHash: true
    }),
    SidebarModule,
    NavbarModule,
    ToastrModule.forRoot(),
    FooterModule,
    FixedPluginModule
  ],
  providers: [SupportTeamService],
  bootstrap: [AppComponent],
})
export class AppModule { }