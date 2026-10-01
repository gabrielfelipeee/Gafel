export const BANK_ACCOUNT_RULES = {
  NAME: {
    MAX_LENGTH: 100,
  },
  INITIAL_BALANCE: {
    MIN: 0,
    MAX: 10_000_000,
  },
} as const;
