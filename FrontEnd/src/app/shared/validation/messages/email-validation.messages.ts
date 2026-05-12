import { tFieldValidationMessages } from '../../types/field-validation-messages.type';
import { EMAIL_RULES } from '../rules/auth-validation.rules';

export const EMAIL_VALIDATION_MESSAGES: tFieldValidationMessages = {
  required: 'Email é obrigatório',
  email: 'Email inválido',
  maxlength: `Email deve ter no máximo ${EMAIL_RULES.MAX_LENGTH} caracteres`,
} as const;
