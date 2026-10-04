import { CategoryType } from '../enums/category-type.enum';

export interface CreateOrEditCategoryRequest {
  name: string;
  icon: string;
  type: CategoryType;
}
