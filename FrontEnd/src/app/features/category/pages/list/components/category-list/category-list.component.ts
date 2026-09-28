import { Component, input, output } from '@angular/core';
import { CategoryType } from '@features/category/enums/category-type.enum';
import { CurrencyPipe, DecimalPipe, NgClass } from '@angular/common';
import { Category } from '@features/category/interfaces/category.interface';
import { IconButtonComponent } from '@shared/components/icon-button/icon-button.component';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import { CategoryAction } from '@features/category/types/category-action.type';
import { EmojiComponent } from '@shared/components/emoji/emoji.component';

@Component({
  selector: 'app-category-list',
  imports: [NgClass, CurrencyPipe, DecimalPipe, IconButtonComponent, EmojiComponent],
  templateUrl: './category-list.component.html',
})
export class CategoryListComponent {
  readonly CategoryType = CategoryType;

  readonly categories = input.required<Category[]>();
  readonly categoryTypeFilter = input.required<CategoryType | undefined>();
  readonly isLoading = input.required<boolean>();

  readonly selectedCategoryIds = input.required<Set<string>>();
  readonly categorySelectionChange = output<{
    id: string;
    selected: boolean;
  }>();
  readonly allCategoriesSelectionChange = output<boolean>();

  readonly actions = input.required<iItemActionData<CategoryAction>[]>();
  readonly action = output<iItemActionEvent<CategoryAction, Category>>();

  onAction(action: CategoryAction, category: Category) {
    this.action.emit({
      action,
      item: category,
    });
  }
}
