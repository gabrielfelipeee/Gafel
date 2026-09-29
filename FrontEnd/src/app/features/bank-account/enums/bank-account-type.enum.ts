import { EnumInfo } from '@shared/types/enum-info.type';

export enum BankAccountType {
  Wallet,
  CheckingAccount,
  SavingsAccount,
  DigitalAccount,
}

export const BankAccountTypeInfo: EnumInfo<BankAccountType> = {
  [BankAccountType.Wallet]: {
    label: 'carteira',
    icon: 'heroBanknotes',
    emoji: 'money',
  },
  [BankAccountType.CheckingAccount]: {
    label: 'conta corrente',
    icon: 'heroBuildingLibrary',
    emoji: 'bank',
  },
  [BankAccountType.SavingsAccount]: {
    label: 'conta poupança',
    icon: 'heroBanknotes',
    emoji: 'pig',
  },
  [BankAccountType.DigitalAccount]: {
    label: 'conta digital',
    icon: 'heroDevicePhoneMobile',
    emoji: 'phone',
  },
};
