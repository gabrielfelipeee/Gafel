import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { catchError } from 'rxjs';
import { iCategory } from '../interfaces/category.interface';
import { CategoryApi } from '../apis/category.api';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';

export const getCategoryResolver: ResolveFn<iCategory> = route => {
  const categoryApi = inject(CategoryApi);
  const toastService = inject(ToastService);

  return categoryApi.getById(route.params['categoryId']).pipe(
    catchError((error: iErrorResponse) => {
      const title =
        error.error?.title && error.error.status != 500
          ? error.error?.title
          : 'Falha ao carregar categoria';

      const message =
        error.error?.detail && error.error.status != 500
          ? error.error.detail
          : 'Tente novamente em alguns instantes.';

      toastService.show('error', title, message);

      throw error;
    }),
  );
};
