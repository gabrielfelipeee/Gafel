export interface PagedResponse<T> {
  items: T[];
  offset: number;
  limit: number;
  total: number;
}
