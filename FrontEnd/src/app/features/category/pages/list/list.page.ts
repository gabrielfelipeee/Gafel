import { Component, computed, inject } from '@angular/core';
import { provideIcons, NgIcon } from '@ng-icons/core';
import { ButtonComponent } from '@shared/components/button/button.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { ItemActionsComponent } from '@shared/components/item-actions/item-actions.component';
import { PaginatorComponent } from '@shared/components/paginator/paginator.component';
import { CategoryApi } from '@features/category/apis/category.api';
import { iCategory } from '@features/category/interfaces/category.interface';
import { iCategoryListFilters } from '@features/category/interfaces/category-list-filters.interface';
import {
  heroEllipsisVertical,
  heroArrowDown,
  heroArrowUp,
  heroHashtag,
  heroPlus,
  heroAcademicCap,
  heroArchiveBox,
  heroArrowTrendingDown,
  heroArrowTrendingUp,
  heroBanknotes,
  heroBeaker,
  heroBolt,
  heroBookOpen,
  heroBriefcase,
  heroBuildingOffice,
  heroCake,
  heroCalendar,
  heroCamera,
  heroChartBar,
  heroChartPie,
  heroClock,
  heroCreditCard,
  heroCurrencyDollar,
  heroEllipsisHorizontalCircle,
  heroFilm,
  heroFire,
  heroGift,
  heroGlobeAmericas,
  heroHeart,
  heroHome,
  heroLockClosed,
  heroMap,
  heroMusicalNote,
  heroPresentationChartLine,
  heroReceiptPercent,
  heroReceiptRefund,
  heroScale,
  heroShieldCheck,
  heroShoppingBag,
  heroShoppingCart,
  heroTruck,
  heroTv,
  heroWallet,
  heroWifi,
  heroTag,
  heroTrash,
  heroPencilSquare,
} from '@ng-icons/heroicons/outline';
import { iItemAction } from '@shared/interfaces/item-action.interface';
import {
  CATEGORY_LIMIT_OPTIONS,
  categoryListQueryParser,
} from '@features/category/parsers/category-list-query.parser';
import { createListResource } from '@shared/query/create-list-resource';

@Component({
  selector: 'app-list-category',
  templateUrl: './list.page.html',
  imports: [ButtonComponent, RouterOutlet, NgIcon, ItemActionsComponent, PaginatorComponent],
  providers: [
    provideIcons({
      heroPlus,
      heroArrowDown,
      heroArrowUp,
      heroHashtag,
      heroAcademicCap,
      heroArchiveBox,
      heroArrowTrendingDown,
      heroArrowTrendingUp,
      heroBanknotes,
      heroBeaker,
      heroBolt,
      heroBookOpen,
      heroBriefcase,
      heroBuildingOffice,
      heroEllipsisVertical,
      heroCake,
      heroCalendar,
      heroCamera,
      heroChartBar,
      heroChartPie,
      heroClock,
      heroCreditCard,
      heroCurrencyDollar,
      heroEllipsisHorizontalCircle,
      heroFilm,
      heroFire,
      heroGift,
      heroGlobeAmericas,
      heroHeart,
      heroHome,
      heroLockClosed,
      heroMap,
      heroMusicalNote,
      heroPresentationChartLine,
      heroReceiptPercent,
      heroReceiptRefund,
      heroScale,
      heroShieldCheck,
      heroShoppingBag,
      heroShoppingCart,
      heroPencilSquare,
      heroTruck,
      heroTrash,
      heroTv,
      heroWallet,
      heroWifi,
      heroTag,
    }),
  ],
})
export class ListPage {
  readonly limits = CATEGORY_LIMIT_OPTIONS;

  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly categoryApi = inject(CategoryApi);

  readonly categoryActions: iItemAction[] = [
    {
      label: 'Editar',
      icon: 'heroPencilSquare',
      callback: this.editCategory,
    },
    {
      label: 'Excluir',
      icon: 'heroTrash',
      callback: this.deleteCategory,
      hoverClass: 'hover:bg-error/10 hover:text-error',
    },
  ];

  private readonly categoryList = createListResource<iCategoryListFilters, iCategory>({
    parser: categoryListQueryParser,
    fetch: query => this.categoryApi.getAll(query.offset, query.limit),
  });

  readonly pagination = computed(() => {
    const response = this.categoryList.response();
    return {
      total: response?.total ?? 0,
      offset: response?.offset ?? 0,
      limit: response?.limit ?? 20,
    };
  });
  readonly categories = computed(() => this.categoryList.response()?.items ?? []);
  readonly onOffsetChange = this.categoryList.setOffset;
  readonly onLimitChange = this.categoryList.setLimit;

  onCreateCategory(): void {
    this.router.navigate(['nova'], {
      relativeTo: this.activatedRoute,
    });
  }

  private deleteCategory(): void {
    alert('Delete');
  }
  private editCategory(): void {
    alert('Edit');
  }
}
