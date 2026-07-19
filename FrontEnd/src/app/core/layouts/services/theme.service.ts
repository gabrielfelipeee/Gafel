import { DOCUMENT, inject, Injectable } from '@angular/core';
import { ALL_THEMES, Theme } from '../types/theme.type';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly document = inject(DOCUMENT);

  private readonly themeKey = 'theme';
  private readonly defaultTheme: Theme = 'gafel-light';

  getInitialTheme(): Theme {
    let theme = localStorage.getItem(this.themeKey) as Theme | null;

    if (!theme || !ALL_THEMES.includes(theme)) {
      theme = this.defaultTheme;
      this.saveTheme(theme);
    }

    this.applyTheme(theme);

    return theme;
  }

  saveTheme(theme: Theme): void {
    localStorage.setItem(this.themeKey, theme);
  }

  applyTheme(theme: Theme): void {
    this.document.documentElement.setAttribute('data-theme', theme);
  }
}
