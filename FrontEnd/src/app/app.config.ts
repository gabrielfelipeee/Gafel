import { ApplicationConfig, LOCALE_ID, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { routes } from './app.routes';
import { provideAuth } from '@core/auth/provide-auth';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { setAuthHeaderInterceptor } from '@core/auth/interceptors/set-auth-header.interceptor';

registerLocaleData(localePt);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    {
      provide: LOCALE_ID,
      useValue: 'pt-BR',
    },
    provideAuth(),
    provideHttpClient(withFetch(), withInterceptors([setAuthHeaderInterceptor])),
  ],
};
