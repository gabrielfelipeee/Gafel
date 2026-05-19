import { Component, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MenuFeature } from '@core/layouts/interfaces/features.interface';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  imports: [NgIcon, RouterLink, RouterLinkActive],
})
export class NavigationComponent {
  features = input.required<MenuFeature[]>();

  closeDropdown(): void {
    (document.activeElement as HTMLElement)?.blur();
  }
}
