import { eCategoryType } from '../enums/category-type.enum';

export interface iCategory {
  id: number;
  name: string;
  icon: string;
  type: eCategoryType;
}
