import { inject, Injectable } from '@angular/core';
import { AuthApi } from '../apis/auth.api';
import { tap } from 'rxjs';
import { AuthTokenStorageService } from '../services/auth-token-storage.service';
import { UserStorageService } from '../services/user-storage.service';
import { iUserCredentials } from '../interfaces/user-credentials.interface';
import { CurrentUserStore } from '../stores/current-user.store';
import { iUser } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class LoginFacade {
  private readonly authApi = inject(AuthApi);
  private readonly authTokenStorageService = inject(AuthTokenStorageService);
  private readonly userStorageService = inject(UserStorageService);
  private readonly currentUserStore = inject(CurrentUserStore);

  login(credentials: iUserCredentials) {
    return this.authApi.login(credentials).pipe(
      tap(response => {
        this.authTokenStorageService.set(response.tokens.accessToken);

        const user: iUser = { fullName: response.fullName };

        this.userStorageService.set(user);
        this.currentUserStore.set(user);
      }),
    );
  }
}
