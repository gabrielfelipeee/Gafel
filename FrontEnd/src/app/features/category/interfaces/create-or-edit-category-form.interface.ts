import { FormControl } from '@angular/forms';
import { CategoryType } from '../enums/category-type.enum';

export interface iCreateOrEditCategoryForm {
  name: FormControl<string>;
  icon: FormControl<string>;
  type: FormControl<CategoryType>;
}
