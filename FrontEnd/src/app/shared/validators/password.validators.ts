import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class PasswordValidators {
  static containsDigit(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value as string;

      if (!value) return null;

      return /\d/.test(value) ? null : { requiresNumber: true };
    };
  }

  static containsSpecialCharacter(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value as string;

      if (!value) return null;

      return /[\W_]/.test(value) ? null : { requiresSpecialChar: true };
    };
  }

  static matchPassword(passwordField: string, confirmPasswordField: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const password = group.get(passwordField)?.value;
      const confirmPassword = group.get(confirmPasswordField)?.value;

      if (password !== confirmPassword) {
        group.get(confirmPasswordField)?.setErrors({
          passwordMismatch: true,
        });

        return { passwordMismatch: true };
      }

      return null;
    };
  }
}
