import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordBookmarkComponent } from './word-bookmark.component';

describe('WordBookmarkComponent', () => {
  let component: WordBookmarkComponent;
  let fixture: ComponentFixture<WordBookmarkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WordBookmarkComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordBookmarkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
