export interface iErrorResponse {
  error?: {
    detail: string;
    title: string;
    instance: string;
    status: number;
    errors?: Record<string, string[]>;
  };
}
