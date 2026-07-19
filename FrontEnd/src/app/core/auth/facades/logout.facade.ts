import { inject, Injectable } from '@angular/core';
import { AuthTokenStorageService } from '../services/auth-token-storage.service';
import { UserStorageService } from '../services/user-storage.service';
import { CurrentUserStore } from '../stores/current-user.store';

@Injectable({
  providedIn: 'root',
})
export class LogoutFacade {
  private readonly authTokenStorageService = inject(AuthTokenStorageService);
  private readonly userStorageService = inject(UserStorageService);
  private readonly currentUserStore = inject(CurrentUserStore);

  logout() {
    this.authTokenStorageService.remove();
    this.userStorageService.remove();
    this.currentUserStore.remove();
  }
}
