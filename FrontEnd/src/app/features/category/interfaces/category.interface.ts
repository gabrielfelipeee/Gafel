import { CategoryType } from '../enums/category-type.enum';

export interface Category {
  id: string;
  name: string;
  icon: string;
  type: CategoryType;
}
