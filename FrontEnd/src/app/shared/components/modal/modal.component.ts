import { Component, viewChild, ElementRef, afterNextRender, output } from '@angular/core';
import { provideIcons } from '@ng-icons/core';
import { heroXMark } from '@ng-icons/heroicons/outline';
import { IconButtonComponent } from '../icon-button/icon-button.component';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  imports: [IconButtonComponent],
  providers: [provideIcons({ heroXMark })],
})
export class ModalComponent {
  modalClose = output<void>();

  private readonly modal = viewChild.required<ElementRef<HTMLDialogElement>>('modal');

  constructor() {
    afterNextRender(() => this.modal().nativeElement.showModal());
  }

  protected onModalClose(): void {
    this.modalClose.emit();
  }

  protected close(): void {
    this.modal().nativeElement.close();
  }
}
