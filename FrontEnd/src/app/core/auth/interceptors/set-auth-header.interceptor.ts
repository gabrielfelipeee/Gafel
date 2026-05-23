import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthTokenStorageService } from '../services/auth-token-storage.service';
import { CurrentUserStore } from '../stores/current-user.store';

export const setAuthHeaderInterceptor: HttpInterceptorFn = (req, next) => {
  const currentUserStore = inject(CurrentUserStore);

  if (!currentUserStore.isLoggedIn()) return next(req);

  const authTokenStorageService = inject(AuthTokenStorageService);
  const token = authTokenStorageService.get() as string;

  req = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });

  return next(req);
};
