import { inject } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, switchMap, Observable } from 'rxjs';
import { iPagedResponse } from '../interfaces/paged-response.interface';
import { PAGINATION_CONFIG } from './constants/pagination-config.constant';
import { iPaginationQuery } from '@shared/interfaces/pagination-query.interface';

export function createListResource<TFilters extends object, TItem>(config: {
  parser: (params: Params) => iPaginationQuery<Partial<TFilters>>;
  fetch: (query: iPaginationQuery<Partial<TFilters>>) => Observable<iPagedResponse<TItem>>;
}) {
  const route = inject(ActivatedRoute);
  const router = inject(Router);

  const query$ = route.queryParams.pipe(map(config.parser));

  const filters = toSignal(query$.pipe(map(query => query.filters)), {
    initialValue: config.parser(route.snapshot.queryParams).filters,
  });

  const DEFAULT_RESPONSE: iPagedResponse<TItem> = {
    items: [],
    offset: 0,
    limit: PAGINATION_CONFIG.DEFAULT_LIMIT,
    total: 0,
  };
  const response = toSignal(query$.pipe(switchMap(query => config.fetch(query))), {
    initialValue: DEFAULT_RESPONSE,
  });

  function setFilters(filters: Partial<TFilters>) {
    router.navigate([], {
      relativeTo: route,
      queryParams: {
        ...filters,
        offset: 0,
      },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }

  // helpers de paginação
  function patch(partial: Partial<iPaginationQuery<TFilters>>) {
    router.navigate([], {
      relativeTo: route,
      queryParams: partial,
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }
  function setOffset(offset: number) {
    patch({ offset });
  }

  function setLimit(limit: number) {
    patch({ offset: 0, limit });
  }

  return {
    response,
    filters,
    setOffset,
    setLimit,
    setFilters,
  };
}
