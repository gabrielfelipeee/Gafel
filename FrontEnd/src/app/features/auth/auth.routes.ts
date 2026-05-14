import { Routes } from '@angular/router';

export const AUTH_ROUTES: Routes = [
  {
    path: 'cadastro',
    loadComponent: () => import('./pages/register/register.page').then(m => m.RegisterPage),
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.page').then(m => m.LoginPage),
  },
];
