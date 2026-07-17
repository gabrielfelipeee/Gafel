import { Component, computed, effect, inject, input, Renderer2, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { ControlValueAccessor, FormsModule, NgControl, Validators } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroCheck, heroListBullet, heroXMark } from '@ng-icons/heroicons/outline';
import { tFieldValidationMessages } from '@shared/types/field-validation-messages.type';
import { getControlErrorMessage } from '@shared/validation/utils/get-control-error-message.utils';
import { SelectOption } from '@shared/interfaces/select-option.interface';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';

@Component({
  selector: 'app-custom-select',
  templateUrl: './custom-select.component.html',
  styleUrl: './custom-select.component.scss',
  imports: [NgIcon, FormsModule, NgClass],
  viewProviders: [provideIcons({ heroCheck, heroListBullet, heroXMark })],
})
export class CustomSelectComponent implements ControlValueAccessor {
  label = input.required<string>();
  options = input.required<SelectOption[]>();
  errorMessages = input<tFieldValidationMessages>({});
  bottomSheetTitle = input.required<string>();

  private readonly ngControl = inject(NgControl, { optional: true, self: true });
  private readonly renderer = inject(Renderer2);
  readonly isMobile = inject(BreakpointObserverService).isMobile;

  readonly value = signal<string | number | null>(null);
  readonly isDisabled = signal(false);
  readonly showBottomSheet = signal(false);

  readonly selectedOption = computed(
    () => this.options().find(option => option.value === this.value()) ?? null,
  );
  readonly isRequired = computed(
    () => this.ngControl?.control?.hasValidator(Validators.required) ?? false,
  );

  constructor() {
    if (this.ngControl) this.ngControl.valueAccessor = this;

    effect(() => {
      // Bloqueia o scroll da página enquanto o Bottom Sheet estiver aberto.
      if (this.showBottomSheet()) this.renderer.addClass(document.body, 'overflow-hidden');
      else
        // Libera o scroll quando o Bottom Sheet é fechado.
        this.renderer.removeClass(document.body, 'overflow-hidden');
    });
  }

  private onChange?: (value: string | number | null) => void;
  private onTouched?: () => void;

  writeValue(value: string | number | null): void {
    this.value.set(value);
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
    this.value.set(value);
    this.onChange?.(value);
  }
  handleTouched(): void {
    this.onTouched?.();
  }

  onClear(event: MouseEvent) {
    event.preventDefault();
    event.stopPropagation();

    this.handleChange(null);
    this.handleTouched();
  }

  // Bottom Sheet
  onOpenBottomSheet(): void {
    if (this.isDisabled()) return;

    this.showBottomSheet.set(true);
  }
  onCloseBottomSheet(): void {
    this.showBottomSheet.set(false);
  }
  onSelectOptionBottomSheet(option: SelectOption): void {
    this.handleChange(option.value);
    this.handleTouched();

    this.showBottomSheet.set(false);
  }

  // Erros
  get errorMessage(): string | null {
    return getControlErrorMessage(this.ngControl?.control ?? null, this.errorMessages());
  }
}
