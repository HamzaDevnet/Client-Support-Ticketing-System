import { TestBed } from '@angular/core/testing';

import { SheardServiceService } from './sheard-service.service';

describe('SheardServiceService', () => {
  let service: SheardServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SheardServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
