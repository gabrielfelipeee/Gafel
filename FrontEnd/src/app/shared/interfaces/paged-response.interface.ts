export interface iPagedResponse<T> {
  items: T[];
  offset: number;
  limit: number;
  total: number;
}
