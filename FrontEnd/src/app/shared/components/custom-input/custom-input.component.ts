import { Component, inject, input, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { ControlValueAccessor, FormsModule, NgControl } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroEye, heroEyeSlash } from '@ng-icons/heroicons/outline';
import { tFieldValidationMessages } from '@shared/types/field-validation-messages.type';
import type { MaskitoOptions } from '@maskito/core';
import { MaskitoDirective } from '@maskito/angular';
import { maskitoNumber } from '@maskito/kit';

type InputType = 'text' | 'password' | 'email' | 'currency';

@Component({
  selector: 'app-custom-input',
  templateUrl: './custom-input.component.html',
  styleUrl: './custom-input.component.scss',
  imports: [NgIcon, FormsModule, NgClass, MaskitoDirective],
  viewProviders: [provideIcons({ heroEye, heroEyeSlash })],
})
export class CustomInputComponent implements ControlValueAccessor {
  type = input<InputType>('text');
  icon = input.required<string>();
  label = input.required<string>();
  placeholder = input.required<string>();
  errorMessages = input<tFieldValidationMessages>({});

  readonly currencyMaskOptions: MaskitoOptions = maskitoNumber({
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
    decimalSeparator: ',',
    thousandSeparator: '.',
    min: 0,
  });

  value = '';
  readonly currentType = signal<InputType>(this.type());
  readonly isDisabled = signal(false);

  private ngControl = inject(NgControl, { optional: true, self: true });

  constructor() {
    if (this.ngControl) this.ngControl.valueAccessor = this;
  }

  private onChange?: (value: string | number | null) => void;
  private onTouched?: () => void;

  writeValue(value: string | number | null): void {
    if (value == null) {
      this.value = '';
      return;
    }

    if (this.type() === 'currency') {
      this.value = this.formatCurrency(Number(value));
      return;
    }

    this.value = String(value);
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

  handleInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;

    this.value = value;

    this.onChange?.(this.type() === 'currency' ? this.parseCurrency(value) : value);
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

  // Erros
  get errorMessage(): string | null {
    const control = this.ngControl?.control;

    if (!control || !control.touched || !control.errors) return null;

    // erro vindo da API
    if (control.errors['apiError']) return control.errors['apiError'];

    // erros locais
    for (const key of Object.keys(control.errors)) {
      const message = this.errorMessages()?.[key];

      if (message) return message;
    }

    return 'Campo inválido';
  }

  private parseCurrency(value: string): number | null {
    if (!value.trim()) return null;

    const parsedValue = Number(
      value
        .replace(/\./g, '') // remove separador de milhar
        .replace(',', '.'), // troca separador decimal
    );

    return Number.isNaN(parsedValue) ? null : parsedValue;
  }
  private formatCurrency(value: number): string {
    return value.toLocaleString('pt-BR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }
}
