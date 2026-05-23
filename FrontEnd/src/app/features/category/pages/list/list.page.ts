import { Component, inject, OnInit, signal } from '@angular/core';
import { provideIcons, NgIcon } from '@ng-icons/core';
import { ButtonComponent } from '@shared/components/button/button.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { iCategory } from '@features/category/interfaces/category.interface';
import { PagedResponse } from '@shared/interfaces/paged-response.interface';
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
import { ItemActionsComponent } from '@shared/components/item-actions/item-actions.component';
import { iItemAction } from '@shared/interfaces/item-action.interface';

@Component({
  selector: 'app-list-category',
  templateUrl: './list.page.html',
  imports: [ButtonComponent, RouterOutlet, NgIcon, ItemActionsComponent],
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
export class ListPage implements OnInit {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly categories = signal<PagedResponse<iCategory>>(this.route.snapshot.data['categories']);
  readonly actions: iItemAction[] = [
    {
      label: 'Editar',
      icon: 'heroPencilSquare',
      callback: this.onEdit,
    },
    {
      label: 'Excluir',
      icon: 'heroTrash',
      callback: this.onDelete,
      hoverClass: 'hover:bg-error/10 hover:text-error',
    },
  ];

  ngOnInit(): void {
    console.log(this.categories());
  }

  onNewCategory() {
    this.router.navigate(['nova'], { relativeTo: this.route });
  }

  private onDelete() {
    alert('Delete');
  }
  private onEdit() {
    alert('Edit');
  }
}
