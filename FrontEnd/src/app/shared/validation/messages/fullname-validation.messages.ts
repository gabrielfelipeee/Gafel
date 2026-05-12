import { tFieldValidationMessages } from '../../types/field-validation-messages.type';
import { FULLNAME_RULES } from '../rules/auth-validation.rules';

export const FULLNAME_VALIDATION_MESSAGES: tFieldValidationMessages = {
  required: 'Nome é obrigatório',
  minlength: `Nome deve ter no mínimo ${FULLNAME_RULES.MIN_LENGTH} caracteres`,
  maxlength: `Nome deve ter no máximo ${FULLNAME_RULES.MAX_LENGTH} caracteres`,
} as const;
