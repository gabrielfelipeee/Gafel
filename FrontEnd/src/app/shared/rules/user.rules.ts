export const USER_RULES = {
  FULL_NAME: {
    MIN_LENGTH: 3,
    MAX_LENGTH: 60,
  },
  EMAIL: {
    MAX_LENGTH: 60,
  },
  PASSWORD: {
    MIN_LENGTH: 8,
    MAX_LENGTH: 60,
  },
} as const;
