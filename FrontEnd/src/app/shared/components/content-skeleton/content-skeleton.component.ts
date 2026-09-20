import { Component, computed, input } from '@angular/core';
import { ViewMode } from '@shared/types/view-mode.type';

@Component({
  selector: 'app-content-skeleton',
  templateUrl: './content-skeleton.component.html',
  imports: [],
})
export class ContentSkeletonComponent {
  readonly totalItems = input.required<number>();
  readonly columnsCount = input(3);
  readonly viewMode = input<ViewMode>('list');

  readonly itemSlots = computed(() => Array.from({ length: this.totalItems() }));
  readonly columnSlots = computed(() => Array.from({ length: this.columnsCount() }));
}
