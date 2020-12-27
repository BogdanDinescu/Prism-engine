import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifySourceModalComponent } from './modify-source-modal.component';

describe('ModifySourceModalComponent', () => {
  let component: ModifySourceModalComponent;
  let fixture: ComponentFixture<ModifySourceModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModifySourceModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModifySourceModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
