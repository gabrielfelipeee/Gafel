import { Routes } from '@angular/router';
import { withPaginationQueryGuard } from '@core/auth/guards/with-pagination-query.guard';
import { categoryListQueryParser } from './parsers/category-list-query.parser';

export const CATEGORY_ROUTES: Routes = [
  {
    path: 'categorias',
    loadComponent: () => import('./pages/list/list.page').then(m => m.ListPage),
    canActivate: [withPaginationQueryGuard({ parser: categoryListQueryParser })],
    children: [
      {
        path: 'nova',
        loadComponent: () =>
          import('./pages/create-or-edit/create-or-edit.page').then(m => m.CreateOrEditPage),
      },
    ],
  },
];
