import { Component, inject, signal } from '@angular/core';
import { UserStorageService } from '@core/auth/services/user-storage.service';
import { LogoComponent } from '@core/layouts/components/logo/logo.component';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  heroAdjustmentsHorizontal,
  heroArrowRightOnRectangle,
  heroArrowsRightLeft,
  heroBanknotes,
  heroBars3,
  heroBuildingLibrary,
  heroCalendarDays,
  heroChartBar,
  heroChevronDown,
  heroCog6Tooth,
  heroDocumentText,
  heroFlag,
  heroHeart,
  heroHome,
  heroMoon,
  heroSquares2x2,
  heroSun,
  heroTag,
  heroUser,
  heroWallet,
  heroXMark,
} from '@ng-icons/heroicons/outline';
import { Theme } from '@core/layouts/types/theme.type';
import { ThemeService } from '@core/layouts/services/theme.service';
import { DesktopNavigationComponent } from './components/desktop-navigation/desktop-navigation.component';
import { NAVIGATION_ITEMS } from '@core/layouts/navigation/navigation-items';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';
import { MobileNavigationComponent } from './components/mobile-navigation/mobile-navigation.component';
import { UserMenuComponent } from './components/user-menu/user-menu.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [
    LogoComponent,
    NgIcon,
    DesktopNavigationComponent,
    MobileNavigationComponent,
    UserMenuComponent,
  ],
  providers: [
    provideIcons({
      heroBars3,
      heroSun,
      heroHome,
      heroUser,
      heroSquares2x2,
      heroAdjustmentsHorizontal,
      heroTag,
      heroChevronDown,
      heroXMark,
      heroMoon,
      heroBanknotes,
      heroWallet,
      heroDocumentText,
      heroBuildingLibrary,
      heroArrowsRightLeft,
      heroCalendarDays,
      heroChartBar,
      heroFlag,
      heroHeart,
      heroArrowRightOnRectangle,
      heroCog6Tooth,
    }),
  ],
})
export class HeaderComponent {
  readonly NAVIGATION_ITEMS = NAVIGATION_ITEMS;

  private readonly themeService = inject(ThemeService);

  readonly isMobile = inject(BreakpointObserverService).isMobile;
  readonly fullName = inject(UserStorageService).get()?.fullName;

  readonly theme = signal<Theme>('gafel-light');

  constructor() {
    this.theme.set(this.themeService.getInitialTheme());
  }

  toggleTheme(): void {
    this.theme.update(prev => (prev === 'gafel-light' ? 'gafel-dark' : 'gafel-light'));

    this.themeService.saveTheme(this.theme());
    this.themeService.applyTheme(this.theme());
  }
}
