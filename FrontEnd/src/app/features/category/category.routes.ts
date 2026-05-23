import { Routes } from '@angular/router';
import { getCategoriesResolver } from './resolvers/get-categories.resolver';

export const CATEGORY_ROUTES: Routes = [
  {
    path: 'categorias',
    loadComponent: () => import('./pages/list/list.page').then(m => m.ListPage),
    resolve: { categories: getCategoriesResolver },
    children: [
      {
        path: 'nova',
        loadComponent: () =>
          import('./pages/create-or-edit/create-or-edit.page').then(m => m.CreateOrEditPage),
      },
    ],
  },
];
