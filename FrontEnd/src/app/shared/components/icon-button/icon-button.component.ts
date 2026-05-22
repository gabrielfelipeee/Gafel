import { Component, input, output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
  host: { class: 'block' },
  selector: 'app-icon-button',
  templateUrl: './icon-button.component.html',
  imports: [NgIcon],
})
export class IconButtonComponent {
  clicked = output<void>();
  icon = input.required<string>();

  onClick() {
    this.clicked.emit();
  }
}
