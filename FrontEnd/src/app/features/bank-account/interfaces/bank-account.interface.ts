import { BankAccountType } from '../enums/bank-account-type.enum';

export interface BankAccount {
  id: string;
  name: string;
  initialBalance: number;
  type: BankAccountType;
}
