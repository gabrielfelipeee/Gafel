import { makeEnvironmentProviders } from '@angular/core';
import { provideSetCurrentUser } from '@core/auth/initializers/set-current-user.initializer';

export function provideAuth() {
  return makeEnvironmentProviders([provideSetCurrentUser()]);
}
