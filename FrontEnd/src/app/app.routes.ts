import { Routes } from '@angular/router';
import { guestGuard } from '@core/auth/guards/guest.guard';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES),
    canActivate: [guestGuard],
  },
];
