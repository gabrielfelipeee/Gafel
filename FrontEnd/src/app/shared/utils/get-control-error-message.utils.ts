import { AbstractControl } from '@angular/forms';
import {
  FormFieldErrorMessages,
  FormValidationErrorKey,
} from '@shared/types/form-validation.types';

export function getControlErrorMessage(
  control: AbstractControl | null,
  messages: FormFieldErrorMessages,
): string | null {
  if (!control?.touched || !control.errors) return null;

  // erro que veio da API
  if (control.errors['apiError']) return control.errors['apiError'];

  // erros locais
  for (const key of Object.keys(control.errors)) {
    const message = messages[key as FormValidationErrorKey];

    if (message) return message;
  }

  return 'Campo inválido';
}
