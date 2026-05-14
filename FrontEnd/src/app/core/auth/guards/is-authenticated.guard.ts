import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CurrentUserStore } from '../stores/current-user.store';

export const isAuthenticatedGuard: CanActivateFn = () => {
  const currentUserStore = inject(CurrentUserStore);
  const router = inject(Router);

  if (!currentUserStore.isLoggedIn()) return router.createUrlTree(['/login']);

  return true;
};
