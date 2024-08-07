import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddclientticketComponent } from './addclientticket.component';

describe('AddclientticketComponent', () => {
  let component: AddclientticketComponent;
  let fixture: ComponentFixture<AddclientticketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddclientticketComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddclientticketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
