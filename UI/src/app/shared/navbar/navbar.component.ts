import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { ROUTES } from '../../sidebar/sidebar.component';
import { Router } from '@angular/router';
import { Location} from '@angular/common';
import { UsersService } from 'app/users.service';
import { UserLocalStorageService } from 'app/user-local-storage.service';
import { Users } from 'app/users';
import { SheardServiceService } from 'app/sheard-service.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
    moduleId: module.id,
    selector: 'navbar-cmp',
    templateUrl: 'navbar.component.html'
})

export class NavbarComponent implements OnInit{
    private listTitles: any[];
    location: Location;
    userProfile: Users 
    private nativeElement: Node;
    private toggleButton;
    private sidebarVisible: boolean;
    selectedLang: string = 'en';
;

    public isCollapsed = true;
    @ViewChild("navbar-cmp", {static: false}) button;

    constructor(location:Location, private renderer : Renderer2, 
      private element : ElementRef,
       private router: Router,
       private UserLocalStorageService : UserLocalStorageService,
       private UsersService : UsersService , 
       private SheardServiceService : SheardServiceService ,
       private translate: TranslateService, 
      
      ) {
        this.location = location;
        this.nativeElement = element.nativeElement;
        this.sidebarVisible = false;
    }

    ngOnInit(){
        this.listTitles = ROUTES.filter(listTitle => listTitle);
        var navbar : HTMLElement = this.element.nativeElement;
        this.toggleButton = navbar.getElementsByClassName('navbar-toggle')[0];
        const userId = this.SheardServiceService.getUserId();
        this.translate.setDefaultLang('en');
        this.translate.use(this.selectedLang);
        this.getUserData(userId);
        this.router.events.subscribe((event) => {
          this.sidebarClose();
       });
    }

    onLanguageChange() {
      this.translate.use(this.selectedLang);
    }
  
    
    getUserData(userId: string):void{
      // this.userProfile = {
      //   firstName: "Alanoud",
      //   lastName: "Abdullah",
      //   fullName: "Alanoud Abdullah",
      //   mobileNumber: "1234567890",
      //   email: "john.doe@example.com",
      //   userStatus: 1,
      //   dateOfBirth: new Date("1990-01-01T00:00:00"),
      //   strDateOfBirth: "Monday, January 1, 1990",
      //   address: "123 Main St, Anytown, USA",
      //   password : "",
      //   userId : "" , 
      //   userImage : null ,
      //   userName : " "
        
      // } 
      this.UsersService.getUserInfo(userId).subscribe({
        next: (response)=>{
         console.log(response);
         this.userProfile = response.data; 
        }, 
        error:(error)=>{
          console.log("Error fechting user information",error);
        }
      })
    }

    getTitle(){
      var titlee = this.location.prepareExternalUrl(this.location.path());
      if(titlee.charAt(0) === '#'){
          titlee = titlee.slice( 1 );
      }
      for(var item = 0; item < this.listTitles.length; item++){
          if(this.listTitles[item].path === titlee){
              return this.listTitles[item].title;
          }
      }
      return 'Dashboard';
    }
    sidebarToggle() {
        if (this.sidebarVisible === false) {
            this.sidebarOpen();
        } else {
            this.sidebarClose();
        }
      }
      sidebarOpen() {
          const toggleButton = this.toggleButton;
          const html = document.getElementsByTagName('html')[0];
          const mainPanel =  <HTMLElement>document.getElementsByClassName('main-panel')[0];
          setTimeout(function(){
              toggleButton.classList.add('toggled');
          }, 500);

          html.classList.add('nav-open');
          if (window.innerWidth < 991) {
            mainPanel.style.position = 'fixed';
          }
          this.sidebarVisible = true;
      };
      sidebarClose() {
          const html = document.getElementsByTagName('html')[0];
          const mainPanel =  <HTMLElement>document.getElementsByClassName('main-panel')[0];
          if (window.innerWidth < 991) {
            setTimeout(function(){
              mainPanel.style.position = '';
            }, 500);
          }
          this.toggleButton.classList.remove('toggled');
          this.sidebarVisible = false;
          html.classList.remove('nav-open');
      };
      collapse(){
        this.isCollapsed = !this.isCollapsed;
        const navbar = document.getElementsByTagName('nav')[0];
        console.log(navbar);
        if (!this.isCollapsed) {
          navbar.classList.remove('navbar-transparent');
          navbar.classList.add('bg-white');
        }else{
          navbar.classList.add('navbar-transparent');
          navbar.classList.remove('bg-white');
        }

      }

      UserProfile():void{
        this.router.navigate(['/userprofile']);
      }

      logout():void{
        this.UserLocalStorageService.clearToken();
        this.router.navigate(['/login']);
      }

}
