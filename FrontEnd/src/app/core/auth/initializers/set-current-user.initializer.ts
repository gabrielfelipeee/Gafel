import { inject, provideAppInitializer } from '@angular/core';
import { AuthTokenStorageService } from '@core/auth/services/auth-token-storage.service';
import { UserStorageService } from '@core/auth/services/user-storage.service';
import { CurrentUserStore } from '@core/auth/stores/current-user.store';
import { of } from 'rxjs';

export function provideSetCurrentUser() {
  return provideAppInitializer(() => {
    const authTokenStorageService = inject(AuthTokenStorageService);
    const userStorageService = inject(UserStorageService);
    const currentUserStore = inject(CurrentUserStore);

    const user = userStorageService.get();
    const authTokenIsValid = authTokenStorageService.isValid();

    if (!user || !authTokenIsValid) {
      authTokenStorageService.remove();
      userStorageService.remove();
      currentUserStore.logout();

      return of();
    }

    currentUserStore.set(user);

    return of();
  });
}
