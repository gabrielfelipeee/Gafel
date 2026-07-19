import { Component, computed, effect, ElementRef, inject, viewChild } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import {
  IsActiveMatchOptions,
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';
import {
  BOTTOM_NAVIGATION_ITEMS,
  MORE_NAVIGATION_ITEMS,
} from '@core/layouts/navigation/navigation-items';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, map, startWith } from 'rxjs';
import { IconButtonComponent } from '@shared/components/icon-button/icon-button.component';

@Component({
  selector: 'app-mobile-navigation',
  templateUrl: './mobile-navigation.component.html',
  imports: [NgIcon, RouterLink, RouterLinkActive, IconButtonComponent],
})
export class MobileNavigationComponent {
  readonly BOTTOM_NAVIGATION_ITEMS = BOTTOM_NAVIGATION_ITEMS;
  readonly MORE_NAVIGATION_ITEMS = MORE_NAVIGATION_ITEMS;

  readonly matchOptions: IsActiveMatchOptions = {
    paths: 'exact', // Compara apenas o caminho
    queryParams: 'ignored', // Ignora query params
    fragment: 'ignored', // Ignora fragmentos (#)
    matrixParams: 'ignored', // Ignora matrix params (;)
  };

  private readonly bottomSheet = viewChild.required<ElementRef<HTMLDialogElement>>('bottomSheet');

  private router = inject(Router);
  readonly isMobile = inject(BreakpointObserverService).isMobile;

  private readonly currentUrl = toSignal(
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.router.url),
      startWith(this.router.url),
    ),
    { initialValue: this.router.url },
  );
  readonly isMoreMenuActive = computed(() =>
    this.MORE_NAVIGATION_ITEMS.some(item => this.currentUrl()?.startsWith(`/${item.path}`)),
  );

  constructor() {
    effect(() => {
      if (!this.isMobile() && this.bottomSheet()) this.bottomSheet().nativeElement?.close();
    });
  }

  onOpenMenu(): void {
    this.bottomSheet().nativeElement.showModal();
  }
  onCloseMenu(): void {
    this.bottomSheet().nativeElement.close();
  }
}
