import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { catchError } from 'rxjs';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { BankAccountApi } from '../apis/bank-account.api';
import { iBankAccount } from '../interfaces/bank-account.interface';

export const getBankAccountResolver: ResolveFn<iBankAccount> = route => {
  const bankAccountApi = inject(BankAccountApi);
  const toastService = inject(ToastService);

  return bankAccountApi.getById(route.params['bankAccountId']).pipe(
    catchError((error: iErrorResponse) => {
      const title =
        error.error?.title && error.error.status != 500
          ? error.error?.title
          : 'Falha ao carregar conta bancária';

      const message =
        error.error?.detail && error.error.status != 500
          ? error.error.detail
          : 'Tente novamente em alguns instantes.';

      toastService.show('error', title, message);

      throw error;
    }),
  );
};
