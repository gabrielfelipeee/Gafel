import { Component, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { IsActiveMatchOptions } from '@angular/router';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { LogoComponent } from '@core/layouts/components/logo/logo.component';
import { iFeatureItem } from '@core/layouts/interfaces/features.interface';

@Component({
  host: { class: 'drawer-side' },
  selector: 'app-drawer',
  templateUrl: './drawer.component.html',
  imports: [NgIcon, LogoComponent, RouterLink, RouterLinkActive, NgClass],
})
export class DrawerComponent {
  features = input.required<iFeatureItem[]>();

  readonly matchOptions: IsActiveMatchOptions = {
    paths: 'exact',
    queryParams: 'ignored',
    fragment: 'ignored',
    matrixParams: 'ignored',
  };

  closeDrawer(): void {
    const drawer = document.getElementById('drawer-menu') as HTMLInputElement;
    if (drawer) drawer.checked = false;
  }
}
