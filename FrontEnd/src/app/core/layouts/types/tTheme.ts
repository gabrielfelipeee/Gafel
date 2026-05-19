export const ALL_THEMES = ['gafel-light', 'gafel-dark'] as const;

export type tTheme = (typeof ALL_THEMES)[number];
