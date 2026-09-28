import {
  ApplicationConfig,
  DEFAULT_CURRENCY_CODE,
  LOCALE_ID,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { routes } from './app.routes';
import { provideAuth } from '@core/auth/provide-auth';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { setAuthHeaderInterceptor } from '@core/auth/interceptors/set-auth-header.interceptor';
import { register } from '@fluentui-emoji/angular';
import { FLUENT_EMOJIS } from '@shared/components/emoji/fluent-emoji';

registerLocaleData(localePt);
register(...Object.values(FLUENT_EMOJIS)); // Registra todos os emojis usados pela aplicação

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    {
      provide: LOCALE_ID,
      useValue: 'pt-BR',
    },
    {
      provide: DEFAULT_CURRENCY_CODE,
      useValue: 'BRL',
    },
    provideAuth(),
    provideHttpClient(withFetch(), withInterceptors([setAuthHeaderInterceptor])),
  ],
};
