import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class SheardServiceService {

  constructor() { }

  getApiUrl(): string {
    return 'http://localhost:7109/api';
  }

}