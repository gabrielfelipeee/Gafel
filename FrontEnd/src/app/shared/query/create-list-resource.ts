import { inject } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, switchMap, Observable } from 'rxjs';
import { iPagedResponse } from '../interfaces/paged-response.interface';
import { PAGINATION_CONFIG } from './constants/pagination-config.constant';

interface iPagination {
  offset: number;
  limit: number;
}

export function createListResource<TFilters, TItem>(config: {
  parser: (params: Params) => TFilters & iPagination;
  fetch: (query: TFilters & iPagination) => Observable<iPagedResponse<TItem>>;
}) {
  const route = inject(ActivatedRoute);
  const router = inject(Router);

  const query$ = route.queryParams.pipe(map(config.parser));

  const response = toSignal(query$.pipe(switchMap(query => config.fetch(query))), {
    initialValue: {
      items: [],
      offset: 0,
      limit: PAGINATION_CONFIG.DEFAULT_LIMIT,
      total: 0,
    },
  });

  // helpers de paginação
  function patch(partial: Partial<iPagination>) {
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
    query: query$,
    setOffset,
    setLimit,
  };
}
