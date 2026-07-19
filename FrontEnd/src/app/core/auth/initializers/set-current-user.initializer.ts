import { inject, provideAppInitializer } from '@angular/core';
import { AuthTokenStorageService } from '@core/auth/services/auth-token-storage.service';
import { UserStorageService } from '@core/auth/services/user-storage.service';
import { CurrentUserStore } from '@core/auth/stores/current-user.store';
import { of } from 'rxjs';
import { LogoutFacade } from '../facades/logout.facade';

export function provideSetCurrentUser() {
  return provideAppInitializer(() => {
    const authTokenStorageService = inject(AuthTokenStorageService);
    const userStorageService = inject(UserStorageService);
    const currentUserStore = inject(CurrentUserStore);
    const logoutFacade = inject(LogoutFacade);

    const user = userStorageService.get();
    const authTokenIsValid = authTokenStorageService.isValid();

    if (!user || !authTokenIsValid) logoutFacade.logout();
    else currentUserStore.set(user);

    return of();
  });
}
