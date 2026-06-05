export function createStringParser() {
  return (value: unknown): string | undefined => {
    if (typeof value !== 'string') return undefined;

    const normalized = value.trim();

    return normalized || undefined;
  };
}
