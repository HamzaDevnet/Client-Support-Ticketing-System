import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientticketsComponent } from './clienttickets.component';

describe('ClientticketsComponent', () => {
  let component: ClientticketsComponent;
  let fixture: ComponentFixture<ClientticketsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientticketsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientticketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
