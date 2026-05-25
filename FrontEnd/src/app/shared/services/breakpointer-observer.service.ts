import { BreakpointObserver } from '@angular/cdk/layout';
import { inject, Injectable } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class BreakpointerObserverService {
  private readonly breakpointObserver = inject(BreakpointObserver);

  // Tailwind >= breakpoint
  readonly sm = this.observe('(min-width: 640px)');
  readonly md = this.observe('(min-width: 768px)');
  readonly lg = this.observe('(min-width: 1024px)');
  readonly xl = this.observe('(min-width: 1280px)');
  readonly xxl = this.observe('(min-width: 1536px)');

  private observe(query: string) {
    return toSignal(this.breakpointObserver.observe(query).pipe(map(state => state.matches)), {
      initialValue: false,
    });
  }
}
