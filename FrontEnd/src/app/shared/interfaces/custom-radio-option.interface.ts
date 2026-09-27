export interface CustomRadioOption<T> {
  value: T;
  label: string;
  icon?: string;

  activeClass?: string;
  hoverClass?: string;
}
