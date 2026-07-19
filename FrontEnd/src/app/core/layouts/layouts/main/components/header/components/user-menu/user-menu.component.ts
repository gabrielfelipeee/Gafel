import { Component, inject } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { Router, RouterLink } from '@angular/router';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';
import { NavigationItem } from '@core/layouts/interfaces/navigation.interface';
import { UserStorageService } from '@core/auth/services/user-storage.service';
import { LogoutFacade } from '@core/auth/facades/logout.facade';
import { NavigationItemComponent } from "@core/layouts/components/navigation-item/navigation-item.component";

@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  imports: [NgIcon, RouterLink, NavigationItemComponent],
})
export class UserMenuComponent {
  readonly userMenuItems: NavigationItem[] = [
    {
      label: 'Meu Perfil',
      path: 'meu-perfil',
      icon: 'heroUser',
    },
    {
      label: 'Configurações',
      path: 'configuracoes',
      icon: 'heroCog6Tooth',
    },
  ];

  private router = inject(Router);
  private readonly logoutFacade = inject(LogoutFacade);

  readonly isMobile = inject(BreakpointObserverService).isMobile;
  readonly fullName = inject(UserStorageService).get()?.fullName;

  onLogout() {
    this.logoutFacade.logout();
    this.router.navigate(['/']);
  }
}
