export enum eBankAccountType {
  Wallet,
  CheckingAccount,
  SavingsAccount,
  DigitalAccount,
}

export const BankAccountTypeInfo: Record<eBankAccountType, { label: string; icon: string }> = {
  [eBankAccountType.Wallet]: {
    icon: 'heroWallet',
    label: 'Carteira',
  },
  [eBankAccountType.CheckingAccount]: {
    icon: 'heroBuildingLibrary',
    label: 'Conta Corrente',
  },
  [eBankAccountType.SavingsAccount]: {
    icon: 'heroBanknotes',
    label: 'Conta Poupança',
  },
  [eBankAccountType.DigitalAccount]: {
    icon: 'heroDevicePhoneMobile',
    label: 'Conta Digital',
  },
};
