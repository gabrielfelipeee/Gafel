import { Component, input, output } from '@angular/core';
import { NgClass } from '@angular/common';
import { NgIcon } from '@ng-icons/core';
import { IconButtonComponent } from '../icon-button/icon-button.component';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';

@Component({
  selector: 'app-item-actions',
  templateUrl: './item-actions.component.html',
  imports: [NgIcon, IconButtonComponent, NgClass],
})
export class ItemActionsComponent<TItem, TAction extends string> {
  readonly actions = input.required<iItemActionData<TAction>[]>();
  readonly item = input.required<TItem>();

  readonly clicked = output<iItemActionEvent<TAction, TItem>>();

  onClick(action: iItemActionData<TAction>) {
    (document.activeElement as HTMLElement)?.blur();

    this.clicked.emit({
      action: action.key,
      item: this.item(),
    });
  }
}
