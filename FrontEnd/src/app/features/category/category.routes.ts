import { Routes } from '@angular/router';

export const CATEGORY_ROUTES: Routes = [
  {
    path: 'categorias',
    loadComponent: () => import('./pages/list/list.page').then(m => m.ListPage),
    children: [
      {
        path: 'nova',
        loadComponent: () =>
          import('./pages/create-or-edit/create-or-edit.page').then(m => m.CreateOrEditPage),
      },
    ],
  },
];
