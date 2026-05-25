export interface iPaginationQuery<TFilters> {
  offset: number;
  limit: number;
  filters: TFilters;
}
