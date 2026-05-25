import { inject } from '@angular/core';
import { CanActivateFn, Params, Router } from '@angular/router';

export function withPaginationQueryGuard<TFilters>(config: {
  parser: (params: Params) => { offset: number; limit: number; filters: Partial<TFilters> };
}) {
  const guard: CanActivateFn = (route, state) => {
    const router = inject(Router);

    const raw = route.queryParams;
    const parsed = config.parser(raw);

    const changed =
      Number(raw['offset']) !== parsed.offset || Number(raw['limit']) !== parsed.limit;

    if (changed) {
      return router.createUrlTree([state.url.split('?')[0]], {
        queryParams: {
          offset: parsed.offset,
          limit: parsed.limit,
          ...parsed.filters,
        },
      });
    }
    return true;
  };

  return guard;
}
