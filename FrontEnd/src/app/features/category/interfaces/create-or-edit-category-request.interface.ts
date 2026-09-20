import { CategoryType } from '../enums/category-type.enum';

export interface iCreateOrEditCategoryRequest {
  name: string;
  icon: string;
  type: CategoryType;
}
