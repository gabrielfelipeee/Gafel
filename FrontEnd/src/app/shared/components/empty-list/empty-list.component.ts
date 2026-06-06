import { Component, input } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroMagnifyingGlass } from '@ng-icons/heroicons/outline';

@Component({
  host: {
    class: 'block w-[95vw] max-w-md mx-auto mt-8 text-center',
  },
  selector: 'app-empty-list',
  templateUrl: './empty-list.component.html',
  imports: [NgIcon],
  providers: [provideIcons({ heroMagnifyingGlass })],
})
export class EmptyListComponent {
  title = input.required<string>();
  description = input.required<string>();
}
