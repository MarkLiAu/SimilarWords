import { TestBed } from '@angular/core/testing';

import { WordsDataService } from './words-data.service';

describe('WordsDataService', () => {
  let service: WordsDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WordsDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
