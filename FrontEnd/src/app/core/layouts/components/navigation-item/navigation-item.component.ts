import { Component, input, output } from '@angular/core';
import { NavigationItem } from '@core/layouts/interfaces/navigation.interface';
import { NgIcon } from '@ng-icons/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-navigation-item',
  templateUrl: './navigation-item.component.html',
  imports: [NgIcon, RouterLink, RouterLinkActive],
})
export class NavigationItemComponent {
  readonly item = input.required<NavigationItem>();
  readonly clicked = output<void>();

  onClick() {
    this.clicked.emit();
  }
}
