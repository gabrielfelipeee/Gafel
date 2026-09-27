export interface CustomRadioGroupOption<T> {
  value: T;
  label: string;
  icon?: string;
  variant?: 'success' | 'warning' | 'error';
}
