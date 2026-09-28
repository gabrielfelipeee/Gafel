import { Component, input } from '@angular/core';
import { FluentEmojiDirective } from '@fluentui-emoji/angular';
import { FluentEmojiName } from '@shared/types/fluent-emoji-name.type';
import { FLUENT_EMOJIS } from './fluent-emoji';

@Component({
  selector: 'app-emoji',
  imports: [FluentEmojiDirective],
  templateUrl: './emoji.component.html',
})
export class EmojiComponent {
  readonly name = input.required<FluentEmojiName>();

  readonly emojiName = () => FLUENT_EMOJIS[this.name()].slug;
}
