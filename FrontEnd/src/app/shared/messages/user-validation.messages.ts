import { USER_RULES } from '@shared/rules/user.rules';
import { FormFieldErrorMessages } from '@shared/types/form-validation.types';

export const USER_VALIDATION_MESSAGES = {
  FULL_NAME: {
    required: 'Nome é obrigatório',
    minlength: `Nome deve ter no mínimo ${USER_RULES.FULL_NAME.MIN_LENGTH} caracteres`,
    maxlength: `Nome deve ter no máximo ${USER_RULES.FULL_NAME.MAX_LENGTH} caracteres`,
  } satisfies FormFieldErrorMessages,

  EMAIL: {
    required: 'Email é obrigatório',
    email: 'Email inválido',
    maxlength: `Email deve ter no máximo ${USER_RULES.EMAIL.MAX_LENGTH} caracteres`,
  } satisfies FormFieldErrorMessages,

  PASSWORD: {
    required: 'Senha é obrigatória',
    minlength: `Senha deve ter no mínimo ${USER_RULES.PASSWORD.MIN_LENGTH} caracteres`,
    maxlength: `Senha deve ter no máximo ${USER_RULES.FULL_NAME.MAX_LENGTH} caracteres`,
    requiresNumber: 'Senha deve conter ao menos um número',
    requiresSpecialChar: 'Senha deve conter ao menos um caractere especial',
  } satisfies FormFieldErrorMessages,
} as const;
