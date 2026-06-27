import { Routes } from '@angular/router';
import { withPaginationQueryGuard } from '@core/auth/guards/with-pagination-query.guard';
import { bankAccountListQueryParser } from './parsers/bank-account-list-query.parser';

export const BANK_ACCOUNT_ROUTES: Routes = [
  {
    path: 'contas',
    loadComponent: () => import('./pages/list/list.page').then(m => m.ListPage),
    canActivate: [withPaginationQueryGuard({ parser: bankAccountListQueryParser })],
  },
];
