import { Component, input, output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  imports: [NgIcon],
})
export class ButtonComponent {
  icon = input.required<string>();
  clicked = output<void>();

  onClick() {
    this.clicked.emit();
  }
}
