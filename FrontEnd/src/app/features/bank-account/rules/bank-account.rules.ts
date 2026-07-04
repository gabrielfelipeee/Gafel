export const BANK_ACCOUNT_RULES = {
  NAME: {
    MAX_LENGTH: 100,
  },
  INITIAL_BALANCE: {
    MIN: 0,
  },
} as const;
