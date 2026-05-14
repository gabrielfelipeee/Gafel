import { inject, Injectable } from '@angular/core';
import { AuthApi } from '../apis/auth.api';
import { iRegisterUserRequest } from '../interfaces/register-user-request.interface';
import { tap } from 'rxjs';
import { AuthTokenStorageService } from '../services/auth-token-storage.service';
import { UserStorageService } from '../services/user-storage.service';

@Injectable({
  providedIn: 'root',
})
export class RegisterUserFacade {
  private readonly authApi = inject(AuthApi);
  private readonly authTokenStorageService = inject(AuthTokenStorageService);
  private readonly userStorageService = inject(UserStorageService);

  register(user: iRegisterUserRequest) {
    return this.authApi.register(user).pipe(
      tap(response => {
        this.authTokenStorageService.set(response.tokens.accessToken);
        this.userStorageService.set({ fullName: response.fullName });
      }),
    );
  }
}
