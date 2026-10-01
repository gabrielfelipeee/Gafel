// Recebe qualquer string, extrai os dígitos e devolve a máscara: "12345" -> "123,45"
export function formatCurrencyMask(raw: string): string {
  const digits = raw.replace(/\D/g, '').slice(0, 10);

  if (!digits) return '';

  const padded = digits.padStart(3, '0');
  const integer = padded.slice(0, -2).replace(/^0+(?=\d)/, '');
  const decimal = padded.slice(-2);

  return `${integer.replace(/\B(?=(\d{3})+(?!\d))/g, '.')},${decimal}`;
}

// number -> máscara: 1234.5 -> "1.234,50"
export function numberToCurrencyMask(value: number): string {
  return formatCurrencyMask(value.toFixed(2));
}

// máscara -> number: "1.234,50" -> 1234.5
export function currencyMaskToNumber(mask: string): number | null {
  if (!mask) return null;

  return Number(mask.replace(/\./g, '').replace(',', '.'));
}
