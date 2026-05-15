import { FormGroup } from '@angular/forms';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';

export function applyApiValidationErrors(form: FormGroup, error: iErrorResponse) {
  const apiErrors = error.error?.errors;

  if (!apiErrors) return;

  Object.keys(apiErrors).forEach(field => {
    const control = form.get(field);

    if (!control) return;

    control.setErrors({
      apiError: apiErrors[field][0],
      ...control.errors,
    });
  });

  form.markAllAsTouched();
}
