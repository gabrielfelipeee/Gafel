import { eBankAccountType } from '../enums/bank-account-type.enum';

export interface iCreateOrEditBankAccountRequest {
  name: string;
  initialBalance: number;
  type: eBankAccountType;
}
