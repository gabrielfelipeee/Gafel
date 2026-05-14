import { Component, inject, input, OnInit, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { ControlValueAccessor, FormsModule, NgControl } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroEye, heroEyeSlash } from '@ng-icons/heroicons/outline';
import { tFieldValidationMessages } from '@shared/types/field-validation-messages.type';

type tInputType = 'text' | 'password' | 'number' | 'email';

@Component({
  selector: 'app-custom-input',
  templateUrl: './custom-input.component.html',
  styleUrl: './custom-input.component.scss',
  imports: [NgIcon, FormsModule, NgClass],
  viewProviders: [provideIcons({ heroEye, heroEyeSlash })],
})
export class CustomInputComponent implements ControlValueAccessor, OnInit {
  type = input<tInputType>('text');
  icon = input.required<string>();
  label = input.required<string>();
  placeholder = input.required<string>();
  errorMessages = input<tFieldValidationMessages>({});

  value = '';
  currentType = signal<tInputType>(this.type());
  isDisabled = signal(false);

  private ngControl = inject(NgControl, { optional: true, self: true });

  constructor() {
    if (this.ngControl) this.ngControl.valueAccessor = this;
  }

  ngOnInit(): void {
    this.currentType.set(this.type());
  }

  private onChange?: (value: string) => void;
  private onTouched?: () => void;

  writeValue(value: string | null): void {
    this.value = value ?? '';
  }
  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    this.isDisabled.set(isDisabled);
  }

  handleInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.value = input.value;
    if (this.onChange) this.onChange(this.value);
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
}
