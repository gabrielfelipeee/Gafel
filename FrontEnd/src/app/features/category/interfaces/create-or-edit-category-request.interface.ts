import { eCategoryType } from '../enums/category-type.enum';

export interface iCreateOrEditCategoryRequest {
  name: string;
  icon: string;
  type: eCategoryType;
}
