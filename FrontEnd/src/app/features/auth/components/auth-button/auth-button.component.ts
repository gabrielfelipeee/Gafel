import { Component, input, output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-auth-button',
  templateUrl: './auth-button.component.html',
  imports: [NgIcon],
})
export class AuthButtonComponent {
  prefixIcon = input.required<string>();
  suffixIcon = input.required<string>();

  clicked = output<void>();

  onClick() {
    this.clicked.emit();
  }
}
