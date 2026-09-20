import { Component, input, output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { NgClass } from '@angular/common';

type IconButtonVariant = 'default' | 'success' | 'info' | 'warning' | 'error';

@Component({
  host: { class: 'block' },
  selector: 'app-icon-button',
  templateUrl: './icon-button.component.html',
  imports: [NgIcon, NgClass],
})
export class IconButtonComponent {
  clicked = output<void>();

  icon = input.required<string>();
  iconSize = input<number>(22);

  variant = input<IconButtonVariant>('default');

  disabled = input<boolean>(false);
  selected = input<boolean>(false);

  onClick() {
    if (this.disabled()) return;
    this.clicked.emit();
  }
}
