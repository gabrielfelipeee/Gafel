import { CurrencyPipe, DecimalPipe, NgClass } from '@angular/common';
import { Component, input, output } from '@angular/core';
import { CategoryType } from '@features/category/enums/category-type.enum';
import { Category } from '@features/category/interfaces/category.interface';
import { ItemActionsComponent } from '@shared/components/item-actions/item-actions.component';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import { CategoryAction } from '@features/category/types/category-action.type';
import { EmojiComponent } from '@shared/components/emoji/emoji.component';

@Component({
  selector: 'app-category-grid',
  imports: [CurrencyPipe, DecimalPipe, ItemActionsComponent, NgClass, EmojiComponent, NgClass],
  templateUrl: './category-grid.component.html',
})
export class CategoryGridComponent {
  readonly CategoryType = CategoryType;

  readonly categories = input.required<Category[]>();
  readonly isLoading = input.required<boolean>();

  readonly actions = input.required<iItemActionData<CategoryAction>[]>();
  readonly action = output<iItemActionEvent<CategoryAction, Category>>();

  onAction(event: iItemActionEvent<CategoryAction, Category>) {
    this.action.emit(event);
  }
}
