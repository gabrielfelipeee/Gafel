import { inject } from '@angular/core';
import { catchError, of } from 'rxjs';
import { ResolveFn } from '@angular/router';
import { CategoryApi } from '../apis/category.api';
import { iCategory } from '../interfaces/category.interface';

export const getCategoriesResolver: ResolveFn<iCategory[]> = () => {
  const categoryApi = inject(CategoryApi);

  return categoryApi.getAll().pipe(
    catchError(error => {
      console.error('Erro ao carregar categorias:', error);

      return of([] as iCategory[]);
    }),
  );
};
