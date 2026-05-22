import { Component, inject } from '@angular/core';
import { provideIcons } from '@ng-icons/core';
import { heroPlus, heroXMark } from '@ng-icons/heroicons/outline';
import { ButtonComponent } from '@shared/components/button/button.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-list-category',
  templateUrl: './list.page.html',
  imports: [ButtonComponent, RouterOutlet],
  providers: [provideIcons({ heroPlus, heroXMark })],
})
export class ListPage {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  onNewCategory() {
    this.router.navigate(['nova'], { relativeTo: this.route });
  }
}
