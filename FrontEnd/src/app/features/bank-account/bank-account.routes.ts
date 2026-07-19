import { Routes } from '@angular/router';
import { withPaginationQueryGuard } from '@core/auth/guards/with-pagination-query.guard';
import { bankAccountListQueryParser } from './parsers/bank-account-list-query.parser';
import { getBankAccountResolver } from './resolvers/get-bank-account.resolver';

export const BANK_ACCOUNT_ROUTES: Routes = [
  {
    path: 'contas-bancarias',
    loadComponent: () => import('./pages/list/list.page').then(m => m.ListPage),
    canActivate: [withPaginationQueryGuard({ parser: bankAccountListQueryParser })],
    children: [
      {
        path: 'nova',
        loadComponent: () =>
          import('./pages/create-or-edit/create-or-edit.page').then(m => m.CreateOrEditPage),
      },
      {
        path: ':bankAccountId',
        loadComponent: () =>
          import('./pages/create-or-edit/create-or-edit.page').then(m => m.CreateOrEditPage),
        resolve: { bankAccount: getBankAccountResolver },
      },
    ],
  },
];
