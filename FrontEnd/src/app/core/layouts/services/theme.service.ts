import { DOCUMENT, inject, Injectable } from '@angular/core';
import { ALL_THEMES, tTheme } from '../types/tTheme';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly document = inject(DOCUMENT);

  private readonly themeKey = 'theme';
  private readonly defaultTheme: tTheme = 'gafel-light';

  getInitialTheme(): tTheme {
    let theme = localStorage.getItem(this.themeKey) as tTheme | null;

    if (!theme || !ALL_THEMES.includes(theme)) {
      theme = this.defaultTheme;
      this.saveTheme(theme);
    }

    this.applyTheme(theme);

    return theme;
  }

  saveTheme(theme: tTheme): void {
    localStorage.setItem(this.themeKey, theme);
  }

  applyTheme(theme: tTheme): void {
    this.document.documentElement.setAttribute('data-theme', theme);
  }
}
