import { eBankAccountType } from '../enums/bank-account-type.enum';

export interface iBankAccount {
  id: string;
  name: string;
  initialBalance: number;
  type: eBankAccountType;
}
