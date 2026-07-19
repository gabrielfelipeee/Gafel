import { Component, inject, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { NavigationItem } from '@core/layouts/interfaces/navigation.interface';
import { NgIcon } from '@ng-icons/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';

@Component({
  selector: 'app-navigation-link',
  templateUrl: './navigation-link.component.html',
  imports: [NgIcon, NgClass, RouterLink, RouterLinkActive],
})
export class NavigationLinkComponent {
  readonly item = input.required<NavigationItem>();

  readonly isDesktop = inject(BreakpointObserverService).isDesktop;
}
