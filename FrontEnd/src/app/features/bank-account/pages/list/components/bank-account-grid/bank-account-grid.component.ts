import { CurrencyPipe, NgClass } from '@angular/common';
import { Component, input, output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { ItemActionsComponent } from '@shared/components/item-actions/item-actions.component';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import { BankAccount } from '@features/bank-account/interfaces/bank-account.interface';
import {
  BankAccountType,
  BankAccountTypeInfo,
} from '@features/bank-account/enums/bank-account-type.enum';
import { BankAccountAction } from '@features/bank-account/types/bank-account-action.type';

@Component({
  selector: 'app-bank-account-grid',
  imports: [CurrencyPipe, NgIcon, ItemActionsComponent, NgClass],
  templateUrl: './bank-account-grid.component.html',
})
export class BankAccountGridComponent {
  readonly BankAccountType = BankAccountType;
  readonly BankAccountTypeInfo = BankAccountTypeInfo;

  readonly bankAccounts = input.required<BankAccount[]>();
  readonly isLoading = input.required<boolean>();

  readonly actions = input.required<iItemActionData<BankAccountAction>[]>();
  readonly action = output<iItemActionEvent<BankAccountAction, BankAccount>>();

  onAction(event: iItemActionEvent<BankAccountAction, BankAccount>) {
    this.action.emit(event);
  }
}
