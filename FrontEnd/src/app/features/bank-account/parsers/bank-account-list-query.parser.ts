import { createStringParser } from '@shared/query/parsers/create-string.parser';
import { createQueryParser } from '@shared/query/parsers/create-query.parser';
import { iBankAccountListFilters } from '../interfaces/bank-account-list-filters.interface';

export const BANK_ACCOUNT_LIMIT_OPTIONS = [10, 20, 50, 75];

export const bankAccountListQueryParser = createQueryParser<iBankAccountListFilters>({
  limits: BANK_ACCOUNT_LIMIT_OPTIONS,
  defaultLimit: BANK_ACCOUNT_LIMIT_OPTIONS[0],
  filters: {
    bankAccountName: createStringParser(),
  },
});
