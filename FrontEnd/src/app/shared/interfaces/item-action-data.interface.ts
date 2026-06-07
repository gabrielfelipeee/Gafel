export interface iItemActionData<TKey extends string> {
  label: string;
  icon: string;
  key: TKey;

  hoverClass?: string;
}
