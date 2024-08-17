import { Component, OnInit, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isRTL: boolean = false; 

  constructor(private translateService: TranslateService, private renderer: Renderer2) { }

  ngOnInit() {
    this.translateService.onLangChange.subscribe(() => {
      this.isRTL = this.translateService.currentLang === 'ar'; 
      const body = document.body;

      if (this.isRTL) {
        this.renderer.setAttribute(body, 'dir', 'rtl');
      } else {
        this.renderer.setAttribute(body, 'dir' , 'ltr');
      }
    });
  }
}