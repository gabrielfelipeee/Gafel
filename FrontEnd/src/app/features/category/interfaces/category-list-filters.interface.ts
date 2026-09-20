import { CategoryType } from '../enums/category-type.enum';

export interface CategoryListFilters {
  type?: CategoryType;
  categoryName?: string;
}
