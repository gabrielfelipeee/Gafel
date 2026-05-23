export interface iCustomRadioOption<T> {
  value: T;
  label: string;
  icon?: string;

  activeClass?: string;
  hoverClass?: string;
}
