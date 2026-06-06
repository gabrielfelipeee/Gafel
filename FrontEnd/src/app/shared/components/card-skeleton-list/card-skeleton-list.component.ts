import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-card-skeleton-list',
  templateUrl: './card-skeleton-list.component.html',
  imports: [],
})
export class CardSkeletonListComponent {
  readonly count = input.required<number>();
  readonly items = computed(() => Array.from({ length: this.count() }));
}
