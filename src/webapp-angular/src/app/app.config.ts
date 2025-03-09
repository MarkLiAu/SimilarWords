import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { testDataInterceptor } from './service/test-data.interceptor';
import { OAuthModule } from 'angular-oauth2-oidc';
import { httpauthInterceptor } from './auth-oidc/httpauth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes, withComponentInputBinding()), 
    provideAnimationsAsync(),
    importProvidersFrom(OAuthModule.forRoot()), // Provide OAuth module globally
    provideHttpClient( withInterceptors([testDataInterceptor, httpauthInterceptor]) )
  ]
};
