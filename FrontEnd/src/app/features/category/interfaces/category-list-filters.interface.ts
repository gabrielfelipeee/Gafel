import { eCategoryType } from '../enums/category-type.enum';

export interface iCategoryListFilters {
  type?: eCategoryType;
  categoryName?: string;
}
