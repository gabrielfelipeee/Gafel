import { FormGroup } from '@angular/forms';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';

export function mapApiErrorsToForm(form: FormGroup, error: iErrorResponse) {
  const apiErrors = error.error?.errors;

  if (!apiErrors) return;

  Object.keys(apiErrors).forEach(field => {
    const control = form.get(field);

    if (!control) return;

    control.setErrors({
      ...control.errors,
      apiError: apiErrors[field][0],
    });
  });

  form.markAllAsTouched();
}
