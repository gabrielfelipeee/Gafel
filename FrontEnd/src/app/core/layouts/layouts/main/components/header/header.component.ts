import { Component, inject, signal } from '@angular/core';
import { UserStorageService } from '@core/auth/services/user-storage.service';
import { LogoComponent } from '@core/layouts/components/logo/logo.component';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  heroAdjustmentsHorizontal,
  heroBars3,
  heroChevronDown,
  heroMoon,
  heroSquares2x2,
  heroSun,
  heroTag,
  heroUser,
  heroXMark,
} from '@ng-icons/heroicons/outline';
import { tTheme } from '@core/layouts/types/tTheme';
import { ThemeService } from '@core/layouts/services/theme.service';
import {
  iFeatureGroup,
  iFeatureItem,
  MenuFeature,
} from '@core/layouts/interfaces/features.interface';
import { DrawerComponent } from './components/drawer/drawer.component';
import { NavigationComponent } from './components/navigation/navigation.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [LogoComponent, NgIcon, DrawerComponent, NavigationComponent],
  providers: [
    provideIcons({
      heroBars3,
      heroSun,
      heroUser,
      heroSquares2x2,
      heroAdjustmentsHorizontal,
      heroTag,
      heroChevronDown,
      heroXMark,
      heroMoon,
    }),
  ],
})
export class HeaderComponent {
  readonly fullName = inject(UserStorageService).get()?.fullName;
  private readonly themeService = inject(ThemeService);

  theme = signal<tTheme>('gafel-light');

  readonly features: MenuFeature[] = [
    {
      label: 'Dashboard',
      path: 'dashboard',
      icon: 'heroSquares2x2',
    } as iFeatureItem,

    {
      label: 'Planejamento',
      icon: 'heroAdjustmentsHorizontal',
      features: [
        {
          label: 'Categorias',
          path: 'categorias',
          icon: 'heroTag',
        },
      ],
    } as iFeatureGroup,
  ];

  readonly featuresList: iFeatureItem[] = this.features.flatMap(item =>
    'features' in item ? item.features : item,
  );

  constructor() {
    this.theme.set(this.themeService.getInitialTheme());
  }

  toggleTheme(): void {
    this.theme.update(prev => (prev === 'gafel-light' ? 'gafel-dark' : 'gafel-light'));

    this.themeService.saveTheme(this.theme());
    this.themeService.applyTheme(this.theme());
  }
}
