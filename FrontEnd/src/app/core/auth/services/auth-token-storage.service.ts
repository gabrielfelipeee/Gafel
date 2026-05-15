import { inject, Injectable } from '@angular/core';
import { STORAGE } from '../tokens/storage.token';
import { jwtDecode } from 'jwt-decode';
import { iJwtPayload } from '../interfaces/jwt-payload.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthTokenStorageService {
  private readonly storageKey = 'auth-token';
  private readonly storage = inject(STORAGE);

  set(token: string): void {
    this.storage.setItem(this.storageKey, token);
  }

  get(): string | null {
    return this.storage.getItem(this.storageKey);
  }

  remove(): void {
    this.storage.removeItem(this.storageKey);
  }

  isValid(): boolean {
    const token = this.storage.getItem(this.storageKey);
    if (!token) return false;

    const decoded = this.decode(token);
    if (!decoded) return false;

    const currentTime = Date.now() / 1000;

    return decoded.exp > currentTime;
  }

  private decode(token: string): iJwtPayload | null {
    try {
      return jwtDecode<iJwtPayload>(token);
    } catch {
      return null;
    }
  }
}
