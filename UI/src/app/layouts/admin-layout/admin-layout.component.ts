import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit {

  public isArabic: boolean = false;

  constructor(private translateService: TranslateService) {}
  
  ngOnInit() {
    this.isArabic = this.translateService.currentLang === 'ar';

    this.translateService.onLangChange.subscribe(event => {
      this.isArabic = event.lang === 'ar';
    });
  }

  onLanguageChange(language: string) {
    this.isArabic = language === 'ar';
    this.translateService.use(language);
  }
}