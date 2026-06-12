import { inject, Injectable } from '@angular/core';
import { ToastService } from './toast.service';
import { FormGroup } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class FormValidationService {
  private readonly toastService = inject(ToastService);

  validate(form: FormGroup): boolean {
    if (form.valid) return true;

    form.markAllAsTouched();

    this.toastService.show(
      'warning',
      'Ainda faltam informações',
      'Verifique os campos destacados.',
    );

    return false;
  }
}
