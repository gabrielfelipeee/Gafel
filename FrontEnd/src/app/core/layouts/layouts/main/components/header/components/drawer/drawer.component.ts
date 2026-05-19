import { Component, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { LogoComponent } from '@core/layouts/components/logo/logo.component';
import { iFeatureItem } from '@core/layouts/interfaces/features.interface';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  host: { class: 'drawer-side' },
  selector: 'app-drawer',
  templateUrl: './drawer.component.html',
  imports: [NgIcon, LogoComponent, RouterLink, RouterLinkActive],
})
export class DrawerComponent {
  features = input.required<iFeatureItem[]>();

  closeDrawer(): void {
    const drawer = document.getElementById('drawer-menu') as HTMLInputElement;
    if (drawer) drawer.checked = false;
  }
}
