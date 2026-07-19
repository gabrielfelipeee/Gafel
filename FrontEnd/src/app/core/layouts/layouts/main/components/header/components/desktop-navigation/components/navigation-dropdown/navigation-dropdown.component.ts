import { Component, computed, inject, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { NavigationGroup } from '@core/layouts/interfaces/navigation.interface';
import { NgIcon } from '@ng-icons/core';
import { NavigationEnd, Router } from '@angular/router';
import { NavigationItemComponent } from '../../../../../../../../components/navigation-item/navigation-item.component';
import { filter, map, startWith } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';

@Component({
  selector: 'app-navigation-dropdown',
  templateUrl: './navigation-dropdown.component.html',
  imports: [NgIcon, NgClass, NavigationItemComponent],
})
export class NavigationDropDownComponent {
  readonly item = input.required<NavigationGroup>();

  private readonly router = inject(Router);
  readonly isDesktop = inject(BreakpointObserverService).isDesktop;

  private readonly currentUrl = toSignal(
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.router.url),
      startWith(this.router.url),
    ),
    { initialValue: this.router.url },
  );
  readonly isDropDownActive = computed(() =>
    this.item().items.some(item => this.currentUrl()?.startsWith(`/${item.path}`)),
  );

  onCloseDropdown() {
    const active = document.activeElement;
    if (active instanceof HTMLElement) active.blur();
  }
}
