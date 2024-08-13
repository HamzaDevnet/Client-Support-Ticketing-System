import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClienttickectdetailsComponent } from './clienttickectdetails.component';

describe('ClienttickectdetailsComponent', () => {
  let component: ClienttickectdetailsComponent;
  let fixture: ComponentFixture<ClienttickectdetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClienttickectdetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClienttickectdetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
