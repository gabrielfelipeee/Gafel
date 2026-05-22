import { FormControl } from '@angular/forms';
import { eCategoryType } from '../enums/category-type.enum';

export interface iCreateOrEditCategoryForm {
  name: FormControl<string>;
  icon: FormControl<string>;
  type: FormControl<eCategoryType>;
}
