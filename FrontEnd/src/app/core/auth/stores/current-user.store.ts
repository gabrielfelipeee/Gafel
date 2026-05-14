import { Injectable, signal, computed } from '@angular/core';
import { iUser } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class CurrentUserStore {
  private readonly state = signal<iUser | null>(null);

  user = computed(() => this.state()); // Usuário Logado

  isLoggedIn = computed(() => this.user() !== null);

  set(user: iUser) {
    this.state.set(user);
  }

  logout() {
    this.state.set(null);
  }
}
