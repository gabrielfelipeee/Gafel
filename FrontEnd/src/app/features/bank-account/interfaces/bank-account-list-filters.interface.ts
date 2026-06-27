import { eBankAccountType } from '../enums/bank-account-type.enum';

export interface iBankAccountListFilters {
  type?: eBankAccountType;
  bankAccountName?: string;
}
