import { Component, OnInit } from '@angular/core';
import { UserType } from 'app/enums/user.enum';
import { SheardServiceService } from 'app/sheard-service.service';


export interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    userType: UserType ;
}

export const ROUTES: RouteInfo[] = [
    { path: '/dashboard',     title: 'Dashboard',         icon:'nc-bank',       class: '' , userType: UserType.Manager },
    { path: '/support',         title: 'Support Members',      icon:'nc-single-02',    class: '' ,  userType: UserType.Manager},
    { path: '/tickets',          title: 'Tickets',           icon:'nc-paper',      class: '' ,  userType: UserType.Manager},
    { path: '/client',          title: 'Clients',           icon:'nc-single-02',      class: '' ,  userType: UserType.Manager},
    { path: '/openticket',          title: 'Assigned Tickets',           icon:'nc-paper',      class: '',  userType: UserType.Support },
    { path: '/myTickets',          title: 'My Tickets',           icon:'nc-paper',      class: '' ,  userType: UserType.Client},

    
  
];

@Component({
    moduleId: module.id,
    selector: 'sidebar-cmp',
    templateUrl: 'sidebar.component.html',
})

export class SidebarComponent implements OnInit {
    public menuItems: any[];
    public userType : UserType ;

    constructor(private SheardServiceService : SheardServiceService){
        this.userType = this.SheardServiceService.getUserType();
        
    }

    ngOnInit() {
        this.menuItems = ROUTES.filter(menuItem => menuItem);
    }


}
