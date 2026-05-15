import { inject, Injectable } from '@angular/core';
import { STORAGE } from '../tokens/storage.token';
import { iUser } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class UserStorageService {
  private readonly storageKey = 'auth-user';
  private readonly storage = inject(STORAGE);

  get(): iUser | null {
    const string = this.storage.getItem(this.storageKey);
    if (!string) return null;

    return JSON.parse(string) as iUser;
  }

  set(user: iUser): void {
    const json = JSON.stringify(user);

    this.storage.setItem(this.storageKey, json);
  }

  remove(): void {
    this.storage.removeItem(this.storageKey);
  }
}
