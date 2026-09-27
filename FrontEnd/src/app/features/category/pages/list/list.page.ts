import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { provideIcons } from '@ng-icons/core';
import {
  heroArrowDown,
  heroArrowUp,
  heroEllipsisVertical,
  heroFunnel,
  heroListBullet,
  heroPencil,
  heroPlus,
  heroSquares2x2,
  heroTrash,
  heroXMark,
} from '@ng-icons/heroicons/outline';
import { ButtonComponent } from '@shared/components/button/button.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginatorComponent } from '@shared/components/paginator/paginator.component';
import { CategoryApi } from '@features/category/apis/category.api';
import { Category } from '@features/category/interfaces/category.interface';
import { CategoryListFilters } from '@features/category/interfaces/category-list-filters.interface';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import {
  CATEGORY_LIMIT_OPTIONS,
  categoryListQueryParser,
} from '@features/category/parsers/category-list-query.parser';
import { createListResource } from '@shared/query/create-list-resource';
import { CustomRadioGroupComponent } from '@shared/components/custom-radio-group/custom-radio-group.component';
import { CustomRadioOption } from '@shared/interfaces/custom-radio-option.interface';
import { CategoryType } from '@features/category/enums/category-type.enum';
import { CATEGORY_ICONS } from '@features/category/contants/category-icons.constant';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { EmptyListComponent } from '@shared/components/empty-list/empty-list.component';
import { ConfirmationModalService } from '@shared/services/confirmation-modal.service';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { RefreshService } from '@shared/services/refresh.service';
import { REFRESH_KEYS } from '@shared/constants/refresh-keys.constant';
import { ApiErrorHandlerService } from '@shared/services/api-error-handler.service';
import { IconButtonComponent } from '@shared/components/icon-button/icon-button.component';
import { CategoryGridComponent } from './components/category-grid/category-grid.component';
import { ViewMode } from '@shared/types/view-mode.type';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { CategoryAction } from '@features/category/types/category-action.type';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';
import { ContentSkeletonComponent } from '@shared/components/content-skeleton/content-skeleton.component';

@Component({
  selector: 'app-category-list-page',
  templateUrl: './list.page.html',
  imports: [
    ButtonComponent,
    RouterOutlet,
    PaginatorComponent,
    CustomRadioGroupComponent,
    FormsModule,
    ReactiveFormsModule,
    EmptyListComponent,
    ContentSkeletonComponent,
    IconButtonComponent,
    CategoryGridComponent,
    CategoryListComponent,
  ],
  providers: [
    provideIcons({
      ...CATEGORY_ICONS,
      heroFunnel,
      heroTrash,
      heroPlus,
      heroArrowUp,
      heroArrowDown,
      heroEllipsisVertical,
      heroListBullet,
      heroSquares2x2,
      heroPencil,
      heroXMark,
    }),
  ],
})
export class CategoryListPage implements OnInit {
  readonly CATEGORY_LIMIT_OPTIONS = CATEGORY_LIMIT_OPTIONS;

  readonly actions: iItemActionData<CategoryAction>[] = [
    {
      key: 'edit',
      label: 'Editar',
      icon: 'heroPencil',
    },
    {
      key: 'delete',
      label: 'Excluir',
      icon: 'heroTrash',
      hoverClass: 'hover:bg-error/10 hover:text-error',
    },
  ];
  readonly categoryTypeOptions: CustomRadioOption<CategoryType | undefined>[] = [
    {
      value: undefined,
      label: 'todas',
      activeClass: 'border-transparent bg-base-300',
      hoverClass: 'hover:border-secondary',
    },
    {
      value: CategoryType.Income,
      label: 'receita',
      icon: 'heroArrowUp',
    },
    {
      value: CategoryType.Expense,
      label: 'despesa',
      icon: 'heroArrowDown',
      activeClass: 'border-transparent bg-error text-error-content',
      hoverClass: 'hover:border-error hover:text-error',
    },
  ];

  private readonly router = inject(Router);
  private readonly categoryApi = inject(CategoryApi);
  private readonly confirmationModalService = inject(ConfirmationModalService);
  private readonly toastService = inject(ToastService);
  private readonly refreshService = inject(RefreshService);
  private readonly apiErrorHandlerService = inject(ApiErrorHandlerService);
  readonly route = inject(ActivatedRoute);
  readonly isMobile = inject(BreakpointObserverService).isMobile;

  readonly searchControl = new FormControl('', { nonNullable: true });
  readonly viewMode = signal<ViewMode>('list');

  readonly selectedCategoryIds = new Set<string>();

  private readonly categoryList = createListResource<CategoryListFilters, Category>({
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

      if (this.searchControl.value !== categoryName)
        this.searchControl.setValue(categoryName, { emitEvent: false });
    });
    effect(() => {
      if (this.isMobile()) this.viewMode.set('grid');
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

  onCategorySelectionChange(result: { id: string; selected: boolean }): void {
    if (result?.selected) this.selectedCategoryIds.add(result.id);
    else this.selectedCategoryIds.delete(result.id);
  }
  onAllCategoriesSelectionChange(selected: boolean): void {
    if (selected)
      this.response().items.forEach(category => this.selectedCategoryIds.add(category.id));
    else this.selectedCategoryIds.clear();
  }

  onCategoryTypeChange(type?: CategoryType): void {
    this.categoryList.setFilters({ type });
  }
  onViewModeChange(mode: ViewMode): void {
    this.viewMode.set(mode);
  }

  async handleCategoryAction(event: iItemActionEvent<CategoryAction, Category>) {
    switch (event.action) {
      case 'edit':
        this.onCreateOrEditCategory(event.item);
        break;

      case 'delete':
        await this.deleteCategory(event.item);
        break;
    }
  }
  onCreateOrEditCategory(category?: Category): void {
    this.router.navigate([category?.id ?? 'nova'], {
      relativeTo: this.route,
      queryParamsHandling: 'preserve',
    });
  }
  private async deleteCategory(category: Category) {
    const confirm = await this.confirmationModalService.show({
      title: 'Excluir categoria',
      message: `Deseja realmente excluir "${category.name}"? Essa ação não poderá ser desfeita.`,
      type: 'danger',
    });

    if (!confirm) return;

    this.categoryApi.delete(category.id).subscribe({
      next: () => {
        this.toastService.show(
          'success',
          'Categoria excluída',
          'Sua categoria foi excluída com sucesso.',
        );
        this.refreshService.trigger(REFRESH_KEYS.CATEGORIES);
      },
      error: (error: iErrorResponse) => {
        this.apiErrorHandlerService.show(
          error,
          'Não foi possível excluir a categoria',
          'Tivemos um problema ao excluir sua categoria. Tente novamente em alguns instantes.',
        );
      },
    });
  }

  onDeleteSelectedCategories() {
    console.log();
  }
}
