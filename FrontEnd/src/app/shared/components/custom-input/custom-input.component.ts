import { Component, inject, input, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { ControlValueAccessor, FormsModule, NgControl } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroEye, heroEyeSlash } from '@ng-icons/heroicons/outline';
import { getControlErrorMessage } from '@shared/utils/get-control-error-message.utils';
import { currencyMaskToNumber, numberToCurrencyMask } from '@shared/utils/currency-mask.utils';
import { CurrencyMaskDirective } from '@shared/directives/currency-mask.directive';
import { FormFieldErrorMessages } from '@shared/types/form-validation.types';

type InputType = 'text' | 'password' | 'email' | 'currency';

@Component({
  selector: 'app-custom-input',
  templateUrl: './custom-input.component.html',
  styleUrl: './custom-input.component.scss',
  imports: [NgIcon, FormsModule, NgClass, CurrencyMaskDirective],
  viewProviders: [provideIcons({ heroEye, heroEyeSlash })],
})
export class CustomInputComponent implements ControlValueAccessor {
  type = input<InputType>('text');
  icon = input.required<string>();
  label = input.required<string>();
  placeholder = input.required<string>();
  errorMessages = input<FormFieldErrorMessages>();

  private ngControl = inject(NgControl, { optional: true, self: true });
  private onChange?: (value: string | number | null) => void;
  private onTouched?: () => void;

  readonly value = signal('');
  readonly isDisabled = signal(false);
  readonly currentType = signal<InputType>(this.type());

  constructor() {
    if (this.ngControl) this.ngControl.valueAccessor = this;
  }

  // #region ControlValueAccessor
  writeValue(value: string | number | null): void {
    if (value == null || value === '') {
      this.value.set('');
      return;
    }

    this.value.set(
      this.type() === 'currency' ? numberToCurrencyMask(Number(value)) : String(value),
    );
  }
  registerOnChange(fn: (value: string | number | null) => void): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    this.isDisabled.set(isDisabled);
  }
  // #endregion ControlValueAccessor

  handleInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;

    this.value.set(value);
    this.onChange?.(this.type() === 'currency' ? currencyMaskToNumber(value) : value);
  }
  handleTouched(): void {
    if (this.onTouched) this.onTouched();
  }

  // Senha
  onTogglePassword(event: MouseEvent) {
    event.preventDefault();
    event.stopPropagation();

    this.currentType.update(type => (type === 'password' ? 'text' : 'password'));
  }

  get errorMessage(): string | null {
    if (!this.errorMessages()) return null;

    return getControlErrorMessage(this.ngControl?.control ?? null, this.errorMessages()!);
  }
}
