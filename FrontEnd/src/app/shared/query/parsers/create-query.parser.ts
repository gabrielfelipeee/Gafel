import { Params } from '@angular/router';
import { iPaginationQuery } from '@shared/interfaces/pagination-query.interface';
import { PAGINATION_CONFIG } from '../constants/pagination-config.constant';

type QueryParamParser<T> = (value: unknown) => T | null | undefined;

type QueryFilterParsers<T> = {
  [K in keyof T]?: QueryParamParser<T[K]>;
};

interface QueryParserOptions<T> {
  limits?: readonly number[];
  defaultLimit?: number;
  filters?: QueryFilterParsers<T>;
}

function parseNumber(value: unknown, fallback: number): number {
  const parsed = Number(value);

  return Number.isFinite(parsed) ? parsed : fallback;
}

function normalizeOffset(offset: number, limit: number): number {
  if (offset < 0) return 0;

  return Math.floor(offset / limit) * limit;
}

export function createQueryParser<T extends object>(schema: QueryParserOptions<T>) {
  const validLimits: number[] = schema.limits?.length
    ? [...schema.limits]
    : [...PAGINATION_CONFIG.DEFAULT_LIMITS];

  const defaultLimit: number = validLimits.includes(
    schema.defaultLimit ?? PAGINATION_CONFIG.DEFAULT_LIMIT,
  )
    ? (schema.defaultLimit ?? PAGINATION_CONFIG.DEFAULT_LIMIT)
    : PAGINATION_CONFIG.DEFAULT_LIMIT;

  return (params: Params): iPaginationQuery<Partial<T>> => {
    const limitRaw = parseNumber(params['limit'], defaultLimit);
    const limit = validLimits.includes(limitRaw) ? limitRaw : defaultLimit;

    const offsetRaw = parseNumber(params['offset'], 0);
    const offset = normalizeOffset(offsetRaw, limit);

    const filters = (
      Object.entries(schema.filters ?? {}) as [keyof T, QueryParamParser<T[keyof T]>][]
    ).reduce((acc, [key, parser]) => {
      const rawValue = params[key as string];
      if (rawValue === undefined || rawValue === null) return acc;

      const parsed = parser(rawValue);
      if (parsed !== undefined && parsed !== null) acc[key] = parsed;

      return acc;
    }, {} as Partial<T>);

    return {
      limit,
      offset,
      filters,
    };
  };
}
