import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CurrentUserStore } from '../stores/current-user.store';

export const guestGuard: CanActivateFn = () => {
  const currentUserStore = inject(CurrentUserStore);
  const router = inject(Router);

  if (currentUserStore.isLoggedIn()) return router.createUrlTree(['/dashboard']);

  return true;
};
