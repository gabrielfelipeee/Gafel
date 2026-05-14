import { FormControl } from '@angular/forms';

export interface iLoginForm {
  email: FormControl<string>;
  password: FormControl<string>;
}
