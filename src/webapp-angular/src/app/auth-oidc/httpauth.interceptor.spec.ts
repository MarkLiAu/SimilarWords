import { TestBed } from '@angular/core/testing';
import { HttpInterceptorFn } from '@angular/common/http';

import { httpauthInterceptor } from './httpauth.interceptor';

describe('httpauthInterceptor', () => {
  const interceptor: HttpInterceptorFn = (req, next) => 
    TestBed.runInInjectionContext(() => httpauthInterceptor(req, next));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(interceptor).toBeTruthy();
  });
});
