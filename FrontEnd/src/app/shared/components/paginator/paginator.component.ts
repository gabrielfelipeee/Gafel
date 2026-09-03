import { Component, computed, inject, input, output } from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  heroChevronLeft,
  heroChevronRight,
  heroChevronDoubleLeft,
  heroChevronDoubleRight,
  heroEllipsisHorizontal,
  heroChevronDown,
} from '@ng-icons/heroicons/outline';
import { PAGINATION_CONFIG } from '@shared/query/constants/pagination-config.constant';
import { IconButtonComponent } from '../icon-button/icon-button.component';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';

@Component({
  host: {
    class: 'sm:fixed sm:bottom-0 sm:left-0 sm:right-0 border-t border-base bg-base-100',
  },
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  imports: [FormsModule, NgIcon, NgClass, IconButtonComponent],
  providers: [
    provideIcons({
      heroChevronLeft,
      heroChevronRight,
      heroChevronDoubleLeft,
      heroChevronDoubleRight,
      heroChevronDown,
      heroEllipsisHorizontal,
    }),
  ],
})
export class PaginatorComponent {
  private readonly isDesktop = inject(BreakpointObserverService).isDesktop;
  private readonly maxVisiblePages = computed(() => (this.isDesktop() ? 5 : 3));

  limits = input<number[]>(PAGINATION_CONFIG.DEFAULT_LIMITS);

  totalItems = input.required<number>();
  offset = input(0);
  limit = input(PAGINATION_CONFIG.DEFAULT_LIMIT);

  offsetChange = output<number>();
  limitChange = output<number>();

  isFirstPage = computed(() => this.currentPage() === 1);
  isLastPage = computed(() => this.totalPages() === this.currentPage());

  currentPage = computed(() => Math.floor(this.offset() / this.limit()) + 1);
  totalPages = computed(() => Math.max(1, Math.ceil(this.totalItems() / this.limit())));

  // range
  startItem = computed(() => (this.totalItems() === 0 ? 0 : this.offset() + 1));
  endItem = computed(() => Math.min(this.offset() + this.limit(), this.totalItems()));

  // Páginas visíveis (máx 5)
  get visiblePages(): number[] {
    const total = this.totalPages();
    const current = this.currentPage();

    if (total <= this.maxVisiblePages()) return Array.from({ length: total }, (_, i) => i + 1);

    let start = current - Math.floor(this.maxVisiblePages() / 2);
    let end = current + Math.floor(this.maxVisiblePages() / 2);

    if (start < 1) {
      start = 1;
      end = this.maxVisiblePages();
    }

    if (end > total) {
      end = total;
      start = total - this.maxVisiblePages() + 1;
    }

    return Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  // navegação
  onPageChange(page: number) {
    const newOffset = (page - 1) * this.limit();

    if (newOffset === this.offset()) return;

    if (page < 1 || page > this.totalPages()) return;

    this.offsetChange.emit(newOffset);
  }

  // alterar tamanho
  changeLimit(size: Event) {
    const parsed = Number(size);
    if (!parsed || parsed <= 0) return;

    this.limitChange.emit(parsed);
  }
}
