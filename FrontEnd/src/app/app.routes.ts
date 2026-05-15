import { Routes } from '@angular/router';
import { guestGuard } from '@core/auth/guards/guest.guard';
import { isAuthenticatedGuard } from '@core/auth/guards/is-authenticated.guard';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES),
    canActivate: [guestGuard],
  },
  {
    path: '',
    canActivate: [isAuthenticatedGuard],
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
      },
    ],
  },
];
