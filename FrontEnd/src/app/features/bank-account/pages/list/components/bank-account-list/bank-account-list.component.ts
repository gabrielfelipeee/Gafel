import { Component, input, output } from '@angular/core';
import { CurrencyPipe, NgClass } from '@angular/common';
import { IconButtonComponent } from '@shared/components/icon-button/icon-button.component';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import {
  BankAccountType,
  BankAccountTypeInfo,
} from '@features/bank-account/enums/bank-account-type.enum';
import { BankAccount } from '@features/bank-account/interfaces/bank-account.interface';
import { BankAccountAction } from '@features/bank-account/types/bank-account-action.type';
import { EmojiComponent } from '@shared/components/emoji/emoji.component';

@Component({
  selector: 'app-bank-account-list',
  imports: [NgClass, CurrencyPipe, IconButtonComponent, EmojiComponent],
  templateUrl: './bank-account-list.component.html',
})
export class BankAccountListComponent {
  readonly BankAccountType = BankAccountType;
  readonly BankAccountTypeInfo = BankAccountTypeInfo;

  readonly bankAccounts = input.required<BankAccount[]>();
  readonly isLoading = input.required<boolean>();

  readonly actions = input.required<iItemActionData<BankAccountAction>[]>();
  readonly action = output<iItemActionEvent<BankAccountAction, BankAccount>>();

  readonly selectedBankAccountIds = input.required<Set<string>>();
  readonly bankAccountSelectionChange = output<{
    id: string;
    selected: boolean;
  }>();
  readonly allBankAccountsSelectionChange = output<boolean>();

  onAction(action: BankAccountAction, bankAccount: BankAccount) {
    this.action.emit({
      action,
      item: bankAccount,
    });
  }
}
