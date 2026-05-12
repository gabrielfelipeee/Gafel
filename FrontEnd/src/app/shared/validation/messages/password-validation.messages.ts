import { tFieldValidationMessages } from '../../types/field-validation-messages.type';
import { PASSWORD_RULES } from '../rules/auth-validation.rules';

export const PASSWORD_VALIDATION_MESSAGES: tFieldValidationMessages = {
  required: 'Senha é obrigatória',
  minlength: `Senha deve ter no mínimo ${PASSWORD_RULES.MIN_LENGTH} caracteres`,
  requiresNumber: 'Senha deve conter ao menos um número',
  requiresSpecialChar: 'Senha deve conter ao menos um caractere especial',
} as const;
