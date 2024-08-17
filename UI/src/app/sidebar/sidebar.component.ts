import { Component, OnInit } from '@angular/core';
import { UserType } from 'app/enums/user.enum';
import { SheardServiceService } from 'app/sheard-service.service';
import { TranslateService } from '@ngx-translate/core'; // Import TranslateService


export interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    userType: UserType ;
}

export const ROUTES: RouteInfo[] = [
    { path: '/dashboard', title: 'dashboard', icon: 'nc-bank', class: '', userType: UserType.Manager },
    { path: '/support', title: 'supportMembers', icon: 'nc-single-02', class: '', userType: UserType.Manager },
    { path: '/tickets', title: 'tickets', icon: 'nc-paper', class: '', userType: UserType.Manager },
    { path: '/client', title: 'clients', icon: 'nc-single-02', class: '', userType: UserType.Manager },
    { path: '/openticket', title: 'assignedTickets', icon: 'nc-paper', class: '', userType: UserType.Support },
    { path: '/myTickets', title: 'myTickets', icon: 'nc-paper', class: '', userType: UserType.Client },
  ];

@Component({
    moduleId: module.id,
    selector: 'sidebar-cmp',
    templateUrl: 'sidebar.component.html',
})

export class SidebarComponent implements OnInit {
    public menuItems: any[];
    public userType : UserType ;
    public isArabic: boolean = false;

    constructor(private SheardServiceService : SheardServiceService, private translateService: TranslateService){
        this.userType = this.SheardServiceService.getUserType();
        
    }

    ngOnInit() {
        this.menuItems = ROUTES.filter(menuItem => menuItem);
        this.isArabic = this.translateService.currentLang === 'ar';
        this.translateService.onLangChange.subscribe(event => {
        this.isArabic = event.lang === 'ar';
    });
    }


}
