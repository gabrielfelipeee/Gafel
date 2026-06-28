import { createEnumParser } from '@shared/query/parsers/create-enum.parser';
import { createStringParser } from '@shared/query/parsers/create-string.parser';
import { createQueryParser } from '@shared/query/parsers/create-query.parser';
import { eBankAccountType } from '../enums/bank-account-type.enum';
import { iBankAccountListFilters } from '../interfaces/bank-account-list-filters.interface';

export const BANK_ACCOUNT_LIMIT_OPTIONS = [10, 20, 50, 75];

export const bankAccountListQueryParser = createQueryParser<iBankAccountListFilters>({
  limits: BANK_ACCOUNT_LIMIT_OPTIONS,
  defaultLimit: 10,
  filters: {
    type: createEnumParser(eBankAccountType),
    bankAccountName: createStringParser(),
  },
});
