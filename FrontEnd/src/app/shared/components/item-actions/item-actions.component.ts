import { Component, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { NgIcon } from '@ng-icons/core';
import { IconButtonComponent } from '../icon-button/icon-button.component';
import { iItemAction } from '@shared/interfaces/item-action.interface';

@Component({
  selector: 'app-item-actions',
  templateUrl: './item-actions.component.html',
  imports: [NgIcon, IconButtonComponent, NgClass],
})
export class ItemActionsComponent {
  readonly actions = input.required<iItemAction[]>();

  onClick(callback: VoidFunction) {
    (document.activeElement as HTMLElement)?.blur();
    callback();
  }
}
