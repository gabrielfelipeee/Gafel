import { AbstractControl } from '@angular/forms';
import { tFieldValidationMessages } from '@shared/types/field-validation-messages.type';

export function getControlErrorMessage(
  control: AbstractControl | null,
  messages: tFieldValidationMessages,
): string | null {
  if (!control?.touched || !control.errors) return null;

  // erro que veio da API
  if (control.errors['apiError']) return control.errors['apiError'];

  // erros locais
  for (const key in control.errors) {
    const message = messages[key];

    if (message) return message;
  }

  return 'Campo inválido';
}
