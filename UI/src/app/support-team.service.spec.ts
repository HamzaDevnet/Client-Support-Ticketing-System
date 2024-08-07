import { TestBed } from '@angular/core/testing';

import { SupportTeamService } from './support-team.service';

describe('SupportTeamService', () => {
  let service: SupportTeamService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SupportTeamService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
