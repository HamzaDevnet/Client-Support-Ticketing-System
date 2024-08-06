import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportTeamMemberComponent } from './support-team-member.component';

describe('SupportTeamMemberComponent', () => {
  let component: SupportTeamMemberComponent;
  let fixture: ComponentFixture<SupportTeamMemberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportTeamMemberComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportTeamMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
