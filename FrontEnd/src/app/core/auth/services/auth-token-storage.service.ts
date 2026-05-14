import { inject, Injectable } from '@angular/core';
import { STORAGE } from '../tokens/storage.token';

@Injectable({
  providedIn: 'root',
})
export class AuthTokenStorageService {
  private readonly storageKey = 'auth-token';
  private readonly storage = inject(STORAGE);

  set(token: string) {
    this.storage.setItem(this.storageKey, token);
  }

  get() {
    return this.storage.getItem(this.storageKey);
  }

  remove() {
    this.storage.removeItem(this.storageKey);
  }

  has() {
    return Boolean(this.storage.getItem(this.storageKey));
  }
}
