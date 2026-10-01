import { Directive, ElementRef, HostListener, inject, input } from '@angular/core';
import { formatCurrencyMask } from '@shared/utils/currency-mask.utils';

@Directive({
  selector: 'input[appCurrencyMask]',
  standalone: true,
})
export class CurrencyMaskDirective {
  readonly maskEnabled = input(false);

  private readonly element = inject(ElementRef<HTMLInputElement>);

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    if (!this.maskEnabled()) return;

    if (event.key !== 'Backspace' && event.key !== 'Delete') return;

    const input = this.element.nativeElement;

    if (input.value === '0,00') {
      event.preventDefault();
      input.value = '';
      input.dispatchEvent(new Event('input', { bubbles: true }));
    }
  }

  @HostListener('input')
  onInput(): void {
    if (!this.maskEnabled()) return;

    const input = this.element.nativeElement;
    input.value = formatCurrencyMask(input.value);
  }
}
