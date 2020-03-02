import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadZipComponent } from './read-zip.component';

describe('ReadZipComponent', () => {
  let component: ReadZipComponent;
  let fixture: ComponentFixture<ReadZipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadZipComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadZipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
