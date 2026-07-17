import { Component, inject, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { NgClass } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MenuFeature } from '@core/layouts/interfaces/features.interface';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  imports: [NgIcon, RouterLink, RouterLinkActive, NgClass],
})
export class NavigationComponent {
  features = input.required<MenuFeature[]>();

  readonly isDesktop = inject(BreakpointObserverService).isDesktop;

  closeDropdown(): void {
    (document.activeElement as HTMLElement)?.blur();
  }
}
