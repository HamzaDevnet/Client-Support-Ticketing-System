import { Routes } from '@angular/router';

import { DashboardComponent } from '../../pages/dashboard/dashboard.component';
import { SupportTeamMemberComponent } from '../../pages/support-team-member/support-team-member.component';
import { TicketComponent } from 'app/pages/ticket/ticket.component';
import { RegisterComponent } from 'app/pages/register/register.component';
import { MyopenticketComponent } from 'app/pages/myopenticket/myopenticket.component';
import { ClientticketsComponent } from 'app/pages/clienttickets/clienttickets.component';
import { ClientComponent } from 'app/pages/client/client.component';
import { ClienttickectdetailsComponent } from 'app/pages/clienttickectdetails/clienttickectdetails.component';
import { UserProfileComponent } from 'app/user-profile/user-profile.component';



export const AdminLayoutRoutes: Routes = [
    { path: 'dashboard',      component: DashboardComponent },
    { path: 'tickets',        component: TicketComponent },
    { path: 'support',        component: SupportTeamMemberComponent },
    {path:'client',          component:ClientComponent},
    { path: 'openticket',     component: MyopenticketComponent },
    { path: 'myTickets',      component: ClientticketsComponent },
    {path: 'ticketdetails',  component:ClienttickectdetailsComponent},
    {path: 'userprofile', component: UserProfileComponent},
      { path: 'ticketdetails',   component: ClienttickectdetailsComponent },


];
