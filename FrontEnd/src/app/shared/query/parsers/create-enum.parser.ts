export function createEnumParser<TEnum extends Record<string, string | number>>(enumObject: TEnum) {
  const validValues = new Set(
    Object.values(enumObject).filter((value): value is number => typeof value === 'number'),
  );

  return (value: unknown): TEnum[keyof TEnum] | undefined => {
    const numeric = Number(value);

    return validValues.has(numeric) ? (numeric as TEnum[keyof TEnum]) : undefined;
  };
}
