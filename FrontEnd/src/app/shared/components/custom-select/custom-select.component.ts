import { Component, computed, inject, input, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { ControlValueAccessor, FormsModule, NgControl, Validators } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroEye, heroEyeSlash, heroXMark } from '@ng-icons/heroicons/outline';
import { tFieldValidationMessages } from '@shared/types/field-validation-messages.type';
import { getControlErrorMessage } from '@shared/validation/utils/get-control-error-message.utils';
import { SelectOption } from '@shared/interfaces/select-option.interface';

@Component({
  selector: 'app-custom-select',
  templateUrl: './custom-select.component.html',
  styleUrl: './custom-select.component.scss',
  imports: [NgIcon, FormsModule, NgClass],
  viewProviders: [provideIcons({ heroEye, heroEyeSlash, heroXMark })],
})
export class CustomSelectComponent implements ControlValueAccessor {
  icon = input.required<string>();
  label = input.required<string>();
  options = input.required<SelectOption[]>();
  errorMessages = input<tFieldValidationMessages>({});

  readonly isRequired = computed(
    () => this.ngControl?.control?.hasValidator(Validators.required) ?? false,
  );

  value: string | number | null = null;
  readonly isDisabled = signal(false);

  private ngControl = inject(NgControl, { optional: true, self: true });

  constructor() {
    if (this.ngControl) this.ngControl.valueAccessor = this;
  }

  private onChange?: (value: string | number | null) => void;
  private onTouched?: () => void;

  writeValue(value: string | number | null): void {
    this.value = value;
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

  handleChange(value: string | number | null): void {
    this.onChange?.(value);
  }
  handleTouched(): void {
    if (this.onTouched) this.onTouched();
  }

  onClear(event: MouseEvent) {
    event.preventDefault();
    event.stopPropagation();

    this.value = null;
  }

  // Erros
  get errorMessage(): string | null {
    return getControlErrorMessage(this.ngControl?.control ?? null, this.errorMessages());
  }
}
