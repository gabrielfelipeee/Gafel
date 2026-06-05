import { Component, effect, inject, OnInit } from '@angular/core';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  heroArrowDown,
  heroArrowUp,
  heroEllipsisVertical,
  heroFunnel,
  heroPencilSquare,
  heroPlus,
  heroTrash,
} from '@ng-icons/heroicons/outline';
import { ButtonComponent } from '@shared/components/button/button.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ItemActionsComponent } from '@shared/components/item-actions/item-actions.component';
import { PaginatorComponent } from '@shared/components/paginator/paginator.component';
import { CategoryApi } from '@features/category/apis/category.api';
import { iCategory } from '@features/category/interfaces/category.interface';
import { iCategoryListFilters } from '@features/category/interfaces/category-list-filters.interface';
import { iItemAction } from '@shared/interfaces/item-action.interface';
import {
  CATEGORY_LIMIT_OPTIONS,
  categoryListQueryParser,
} from '@features/category/parsers/category-list-query.parser';
import { createListResource } from '@shared/query/create-list-resource';
import { CustomRadioGroupComponent } from '@shared/components/custom-radio-group/custom-radio-group.component';
import { iCustomRadioOption } from '@shared/interfaces/custom-radio-option.interface';
import { eCategoryType } from '@features/category/enums/category-type.enum';
import { CATEGORY_ICONS } from '@features/category/contants/category-icons.constant';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  host: { class: 'block relative' },
  selector: 'app-list-category',
  templateUrl: './list.page.html',
  imports: [
    ButtonComponent,
    RouterOutlet,
    NgIcon,
    ItemActionsComponent,
    PaginatorComponent,
    CustomRadioGroupComponent,
    CurrencyPipe,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    provideIcons({
      ...CATEGORY_ICONS,
      heroFunnel,
      heroTrash,
      heroPencilSquare,
      heroPlus,
      heroArrowUp,
      heroArrowDown,
      heroEllipsisVertical,
    }),
  ],
})
export class ListPage implements OnInit {
  readonly CATEGORY_LIMIT_OPTIONS = CATEGORY_LIMIT_OPTIONS;
  readonly eCategoryType = eCategoryType;

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly categoryApi = inject(CategoryApi);

  readonly searchControl = new FormControl('', { nonNullable: true });

  readonly categoryActions: iItemAction[] = [
    {
      label: 'Editar',
      icon: 'heroPencilSquare',
      callback: () => this.editCategory(),
    },
    {
      label: 'Excluir',
      icon: 'heroTrash',
      callback: () => this.deleteCategory(),
      hoverClass: 'hover:bg-error/10 hover:text-error',
    },
  ];
  readonly categoryTypeOptions: iCustomRadioOption<eCategoryType | undefined>[] = [
    {
      value: undefined,
      label: 'todas',
      activeClass: 'border-transparent bg-base-300',
      hoverClass: 'hover:border-secondary',
    },
    {
      value: eCategoryType.Income,
      label: 'receita',
      icon: 'heroArrowUp',
    },
    {
      value: eCategoryType.Expense,
      label: 'despesa',
      icon: 'heroArrowDown',
      activeClass: 'border-transparent bg-error text-error-content',
      hoverClass: 'hover:border-error hover:text-error',
    },
  ];

  private readonly categoryList = createListResource<iCategoryListFilters, iCategory>({
    parser: categoryListQueryParser,
    fetch: ({ offset, limit, filters }) =>
      this.categoryApi.getAll(offset, limit, filters.type, filters.categoryName),
  });
  readonly response = this.categoryList.response;
  readonly onOffsetChange = this.categoryList.setOffset;
  readonly onLimitChange = this.categoryList.setLimit;
  readonly filters = this.categoryList.filters;

  constructor() {
    effect(() => {
      const categoryName = this.filters().categoryName ?? '';

      if (this.searchControl.value === categoryName) return;

      this.searchControl.setValue(categoryName, { emitEvent: false });
    });
  }

  ngOnInit(): void {
    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(value => {
        this.categoryList.setFilters({
          categoryName: value?.trim() || undefined,
        });
      });
  }

  onCreateCategory(): void {
    this.router.navigate(['nova'], { relativeTo: this.route });
  }

  onSelectCategoryType(type?: eCategoryType): void {
    this.categoryList.setFilters({ type });
  }

  private deleteCategory(): void {
    alert('Delete');
  }
  private editCategory(): void {
    alert('Edit');
  }
}
