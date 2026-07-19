export interface NavigationItem {
  label: string;
  path: string;
  icon: string;
}

export interface NavigationGroup {
  label: string;
  icon: string;
  items: NavigationItem[];
}
