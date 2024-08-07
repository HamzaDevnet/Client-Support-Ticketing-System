import { Routes } from '@angular/router';

import { DashboardComponent } from '../../pages/dashboard/dashboard.component';
import { UserComponent } from '../../pages/user/user.component';
import { SupportTeamMemberComponent } from '../../pages/support-team-member/support-team-member.component';
import { TicketComponent } from 'app/pages/ticket/ticket.component';
import { RegisterComponent } from 'app/pages/register/register.component';
import { MyopenticketComponent } from 'app/pages/myopenticket/myopenticket.component';
import { ClientticketsComponent } from 'app/pages/clienttickets/clienttickets.component';


export const AdminLayoutRoutes: Routes = [
    {path:'register',         component:RegisterComponent},
    { path: 'dashboard',      component: DashboardComponent },
    { path: 'user',           component: UserComponent },
    { path: 'tickets',        component: TicketComponent },
    { path: 'support',        component: SupportTeamMemberComponent },
    { path: 'openticket',        component: MyopenticketComponent },
    { path: 'myTickets',        component: ClientticketsComponent },


];
