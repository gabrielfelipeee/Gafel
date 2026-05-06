import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [],
})
export class AppComponent {
  protected readonly title = signal('Gafel');
}
