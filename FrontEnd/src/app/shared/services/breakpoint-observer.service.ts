import { BreakpointObserver } from '@angular/cdk/layout';
import { inject, Injectable } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class BreakpointObserverService {
  private readonly breakpointObserver = inject(BreakpointObserver);

  readonly isMobile = this.observe('(max-width: 639.98px)');
  readonly isTablet = this.observe('(min-width: 640px) and (max-width: 1023.98px)');
  readonly isDesktop = this.observe('(min-width: 1024px)');

  private observe(query: string) {
    return toSignal(this.breakpointObserver.observe(query).pipe(map(state => state.matches)), {
      initialValue: false,
    });
  }
}
