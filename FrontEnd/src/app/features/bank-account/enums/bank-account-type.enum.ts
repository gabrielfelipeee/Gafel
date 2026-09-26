export enum BankAccountType {
  Wallet,
  CheckingAccount,
  SavingsAccount,
  DigitalAccount,
}

export const BankAccountTypeInfo: Record<BankAccountType, { label: string; icon: string }> = {
  [BankAccountType.Wallet]: {
    icon: 'heroWallet',
    label: 'carteira',
  },
  [BankAccountType.CheckingAccount]: {
    icon: 'heroBuildingLibrary',
    label: 'conta corrente',
  },
  [BankAccountType.SavingsAccount]: {
    icon: 'heroBanknotes',
    label: 'conta poupança',
  },
  [BankAccountType.DigitalAccount]: {
    icon: 'heroDevicePhoneMobile',
    label: 'conta digital',
  },
};
