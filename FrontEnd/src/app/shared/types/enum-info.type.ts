import { FluentEmojiName } from './fluent-emoji-name.type';

export type EnumInfo<T extends PropertyKey> = Record<
  T,
  {
    label: string;
    icon?: string;
    emoji?: FluentEmojiName;
  }
>;
