import { computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, switchMap, Observable, tap, finalize, merge, EMPTY } from 'rxjs';
import { iPagedResponse } from '../interfaces/paged-response.interface';
import { PAGINATION_CONFIG } from './constants/pagination-config.constant';
import { iPaginationQuery } from '@shared/interfaces/pagination-query.interface';

export function createListResource<TFilters extends object, TItem>(config: {
  parser: (params: Params) => iPaginationQuery<Partial<TFilters>>;
  fetch: (query: iPaginationQuery<Partial<TFilters>>) => Observable<iPagedResponse<TItem>>;
  refresh$?: Observable<void>;
}) {
  const route = inject(ActivatedRoute);
  const router = inject(Router);

  const query$ = route.queryParams.pipe(map(config.parser));

  const filters = toSignal(query$.pipe(map(query => query.filters)), {
    initialValue: config.parser(route.snapshot.queryParams).filters,
  });

  const pendingRequests = signal(0);
  const isLoading = computed(() => pendingRequests() > 0);

  const DEFAULT_RESPONSE: iPagedResponse<TItem> = {
    items: [],
    offset: 0,
    limit: PAGINATION_CONFIG.DEFAULT_LIMIT,
    total: 0,
  };
  const response = toSignal(
    merge(
      query$,
      config.refresh$?.pipe(map(() => config.parser(route.snapshot.queryParams))) ?? EMPTY,
    ).pipe(
      tap(() => pendingRequests.update(v => v + 1)),
      switchMap(query =>
        config.fetch(query).pipe(finalize(() => pendingRequests.update(v => v - 1))),
      ),
    ),
    {
      initialValue: DEFAULT_RESPONSE,
    },
  );

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
    isLoading,
    setOffset,
    setLimit,
    setFilters,
  };
}
