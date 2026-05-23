import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAuth } from '@core/auth/provide-auth';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { setAuthHeaderInterceptor } from '@core/auth/interceptors/set-auth-header.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideAuth(),
    provideHttpClient(withFetch(), withInterceptors([setAuthHeaderInterceptor])),
  ],
};
