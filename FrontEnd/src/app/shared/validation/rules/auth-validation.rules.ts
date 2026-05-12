export const PASSWORD_RULES = {
  MIN_LENGTH: 8,
} as const;

export const FULLNAME_RULES = {
  MIN_LENGTH: 3,
  MAX_LENGTH: 60,
} as const;

export const EMAIL_RULES = {
  MAX_LENGTH: 60,
} as const;
