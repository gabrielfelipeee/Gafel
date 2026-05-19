import {
  Component,
  HostListener,
  OnDestroy,
  OnInit,
  Renderer2,
  inject,
  signal,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { LogoComponent } from '../../components/logo/logo.component';
import { ThemeService } from '@core/layouts/services/theme.service';

@Component({
  selector: 'app-auth-layout',
  templateUrl: './auth.layout.html',
  styleUrl: './auth.layout.scss',
  imports: [LogoComponent],
})
export class AuthLayout implements OnInit, OnDestroy {
  private readonly DESKTOP_BREAKPOINT = 1024;
  private readonly document = inject(DOCUMENT);
  private readonly renderer = inject(Renderer2);
  private readonly themeService = inject(ThemeService);

  animate = signal(true);
  isMobileOrTablet = this.checkViewport();

  ngOnDestroy(): void {
    this.renderer.removeClass(this.document.body, 'grid-pattern');
  }
  ngOnInit(): void {
    this.themeService.getInitialTheme();
    this.renderer.addClass(this.document.body, 'grid-pattern');
  }

  @HostListener('window:resize')
  onResize(): void {
    const currentIsMobileOrTablet = this.checkViewport();

    if (this.isMobileOrTablet !== currentIsMobileOrTablet) {
      this.animate.set(false);
      requestAnimationFrame(() => this.animate.set(true));

      this.isMobileOrTablet = currentIsMobileOrTablet;
    }
  }

  private checkViewport(): boolean {
    return window.innerWidth < this.DESKTOP_BREAKPOINT;
  }
}
