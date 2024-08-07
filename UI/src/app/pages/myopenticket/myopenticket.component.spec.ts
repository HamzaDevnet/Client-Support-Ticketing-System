import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyopenticketComponent } from './myopenticket.component';

describe('MyopenticketComponent', () => {
  let component: MyopenticketComponent;
  let fixture: ComponentFixture<MyopenticketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyopenticketComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyopenticketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
