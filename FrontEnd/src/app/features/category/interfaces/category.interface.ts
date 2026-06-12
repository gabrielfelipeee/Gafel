import { eCategoryType } from '../enums/category-type.enum';

export interface iCategory {
  id: string;
  name: string;
  icon: string;
  type: eCategoryType;
}
