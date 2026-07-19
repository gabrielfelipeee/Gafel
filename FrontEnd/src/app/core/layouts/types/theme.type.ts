export const ALL_THEMES = ['gafel-light', 'gafel-dark'] as const;

export type Theme = (typeof ALL_THEMES)[number];
