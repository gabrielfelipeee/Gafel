import { Injectable } from '@angular/core';
import { REFRESH_KEYS } from '@shared/constants/refresh-keys.constant';
import { filter, map, Observable, Subject } from 'rxjs';

type tRefreshKey = (typeof REFRESH_KEYS)[keyof typeof REFRESH_KEYS];

@Injectable({
  providedIn: 'root',
})
export class RefreshService {
  private readonly refreshSubject = new Subject<tRefreshKey>();

  trigger(key: tRefreshKey): void {
    this.refreshSubject.next(key);
  }

  on(key: tRefreshKey): Observable<void> {
    return this.refreshSubject.pipe(
      filter(refreshKey => refreshKey === key),
      map(() => void 0),
    );
  }
}
