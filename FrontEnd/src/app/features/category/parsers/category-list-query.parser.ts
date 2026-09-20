import { createEnumParser } from '@shared/query/parsers/create-enum.parser';
import { createStringParser } from '@shared/query/parsers/create-string.parser';
import { CategoryType } from '../enums/category-type.enum';
import { CategoryListFilters } from '../interfaces/category-list-filters.interface';
import { createQueryParser } from '@shared/query/parsers/create-query.parser';

export const CATEGORY_LIMIT_OPTIONS = [10, 20, 50, 75];

export const categoryListQueryParser = createQueryParser<CategoryListFilters>({
  limits: CATEGORY_LIMIT_OPTIONS,
  defaultLimit: 10,
  filters: {
    type: createEnumParser(CategoryType),
    categoryName: createStringParser(),
  },
});
