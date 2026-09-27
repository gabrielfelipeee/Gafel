import { Component, input, output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { NgClass } from '@angular/common';
import { ButtonVariant } from '@shared/types/button-variant.type';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  imports: [NgIcon, NgClass],
})
export class ButtonComponent {
  icon = input.required<string>();
  variant = input<ButtonVariant>('success');
  isLoading = input<boolean>(false);

  clicked = output<void>();

  onClick() {
    this.clicked.emit();
  }
}
