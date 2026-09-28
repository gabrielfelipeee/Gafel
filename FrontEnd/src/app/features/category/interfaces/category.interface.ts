import { CategoryType } from '../enums/category-type.enum';
import { FluentEmojiName } from '@shared/types/fluent-emoji-name.type';

export interface Category {
  id: string;
  name: string;
  icon: FluentEmojiName;
  type: CategoryType;
}
