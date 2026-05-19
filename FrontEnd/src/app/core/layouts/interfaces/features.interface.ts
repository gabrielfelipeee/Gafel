export type MenuFeature = iFeatureItem | iFeatureGroup;

export interface iFeatureItem {
  label: string;
  path: string;
  icon: string;
}

export interface iFeatureGroup {
  label: string;
  icon: string;
  features: iFeatureItem[];
}
