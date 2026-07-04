import { FormControl } from '@angular/forms';
import { eBankAccountType } from '../enums/bank-account-type.enum';

export interface iCreateOrEditBankAccountForm {
  name: FormControl<string>;
  initialBalance: FormControl<number>;
  type: FormControl<eBankAccountType>;
}
