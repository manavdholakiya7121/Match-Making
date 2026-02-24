import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { InitService } from '../core/services/init-service';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';
import { errorInterceptor } from '../core/interceptors/error-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection(),
    provideRouter(routes, withViewTransitions()),
    provideHttpClient(withInterceptors([errorInterceptor])),
    provideAppInitializer(async () => {
      const initService = inject(InitService);

      return new Promise<void>((resolve) => {
        setTimeout(async () => {
          try {
            return lastValueFrom(initService.init());
          }
          finally {
            const splash = document.getElementById('initial-splash');
            if (splash) {
              splash.remove();
            }
            resolve();
          }
        }, 500)
     }) 
    })
  ]
};
