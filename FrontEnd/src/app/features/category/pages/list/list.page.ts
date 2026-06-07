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
import { CurrencyPipe, NgClass } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ItemActionsComponent } from '@shared/components/item-actions/item-actions.component';
import { PaginatorComponent } from '@shared/components/paginator/paginator.component';
import { CategoryApi } from '@features/category/apis/category.api';
import { iCategory } from '@features/category/interfaces/category.interface';
import { iCategoryListFilters } from '@features/category/interfaces/category-list-filters.interface';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
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
import { EmptyListComponent } from '@shared/components/empty-list/empty-list.component';
import { CardSkeletonListComponent } from '@shared/components/card-skeleton-list/card-skeleton-list.component';
import { ConfirmationModalService } from '@shared/services/confirmation-modal.service';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { RefreshService } from '@shared/services/refresh.service';
import { REFRESH_KEYS } from '@shared/constants/refresh-keys.constant';

type tCategoryAction = 'edit' | 'delete';

@Component({
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
    EmptyListComponent,
    CardSkeletonListComponent,
    NgClass,
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
  private readonly confirmationModalService = inject(ConfirmationModalService);
  private readonly toastService = inject(ToastService);
  private readonly refreshService = inject(RefreshService);

  readonly searchControl = new FormControl('', { nonNullable: true });

  readonly categoryActions: iItemActionData<tCategoryAction>[] = [
    {
      key: 'edit',
      label: 'Editar',
      icon: 'heroPencilSquare',
    },
    {
      key: 'delete',
      label: 'Excluir',
      icon: 'heroTrash',
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
    refresh$: this.refreshService.on(REFRESH_KEYS.CATEGORIES),
  });
  readonly response = this.categoryList.response;
  readonly onOffsetChange = this.categoryList.setOffset;
  readonly onLimitChange = this.categoryList.setLimit;
  readonly filters = this.categoryList.filters;
  readonly isLoading = this.categoryList.isLoading;

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
    this.router.navigate(['nova'], { relativeTo: this.route, queryParamsHandling: 'preserve' });
  }

  onSelectCategoryType(type?: eCategoryType): void {
    this.categoryList.setFilters({ type });
  }

  async onCategoryAction(event: iItemActionEvent<tCategoryAction, iCategory>) {
    switch (event.action) {
      case 'edit':
        this.editCategory(event.item);
        break;

      case 'delete':
        await this.deleteCategory(event.item);
        break;
    }
  }

  private async deleteCategory(category: iCategory) {
    const confirm = await this.confirmationModalService.show({
      title: 'Excluir categoria',
      message: `Tem certeza de que deseja excluir a categoria "${category.name}"? Esta ação não poderá ser desfeita.`,
      type: 'danger',
    });
    if (!confirm) return;

    this.categoryApi.delete(category.id).subscribe({
      next: () => {
        this.toastService.show(
          'success',
          'Categoria excluída',
          'A categoria foi excluída com sucesso.',
        );
        this.refreshService.trigger(REFRESH_KEYS.CATEGORIES);
      },
      error: (error: iErrorResponse) => {
        const title = error.error?.title || 'Falha ao excluir categoria';
        const message =
          error.error?.status !== 500 && error.error?.detail
            ? error.error.detail
            : 'Não foi possível excluir a categoria no momento. Tente novamente em alguns instantes.';

        this.toastService.show('error', title, message);
      },
    });
  }
  private editCategory(category: iCategory): void {
    alert(category.name);
  }
}
