import { Component } from '@angular/core';
import { NAVIGATION_ITEMS } from '@core/layouts/navigation/navigation-items';
import { NavigationDropDownComponent } from '@core/layouts/layouts/main/components/header/components/desktop-navigation/components/navigation-dropdown/navigation-dropdown.component';
import { NavigationLinkComponent } from './components/navigation-link/navigation-link.component';

@Component({
  selector: 'app-desktop-navigation',
  templateUrl: './desktop-navigation.component.html',
  imports: [NavigationDropDownComponent, NavigationLinkComponent],
})
export class DesktopNavigationComponent {
  readonly NAVIGATION_ITEMS = NAVIGATION_ITEMS;
}
