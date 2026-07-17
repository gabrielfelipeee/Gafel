import { Component, effect, inject, OnInit } from '@angular/core';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  heroBanknotes,
  heroBuildingLibrary,
  heroDevicePhoneMobile,
  heroFunnel,
  heroPencilSquare,
  heroPlus,
  heroTrash,
  heroWallet,
} from '@ng-icons/heroicons/outline';
import { ButtonComponent } from '@shared/components/button/button.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { CurrencyPipe, NgClass } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ItemActionsComponent } from '@shared/components/item-actions/item-actions.component';
import { PaginatorComponent } from '@shared/components/paginator/paginator.component';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import { createListResource } from '@shared/query/create-list-resource';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { EmptyListComponent } from '@shared/components/empty-list/empty-list.component';
import { CardSkeletonListComponent } from '@shared/components/card-skeleton-list/card-skeleton-list.component';
import { RefreshService } from '@shared/services/refresh.service';
import { REFRESH_KEYS } from '@shared/constants/refresh-keys.constant';
import {
  BANK_ACCOUNT_LIMIT_OPTIONS,
  bankAccountListQueryParser,
} from '@features/bank-account/parsers/bank-account-list-query.parser';
import { iBankAccountListFilters } from '@features/bank-account/interfaces/bank-account-list-filters.interface';
import { iBankAccount } from '@features/bank-account/interfaces/bank-account.interface';
import { BankAccountApi } from '@features/bank-account/apis/bank-account.api';
import {
  BankAccountTypeInfo,
  eBankAccountType,
} from '@features/bank-account/enums/bank-account-type.enum';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { ConfirmationModalService } from '@shared/services/confirmation-modal.service';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { ApiErrorHandlerService } from '@shared/services/api-error-handler.service';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';

type tBankAccountAction = 'edit' | 'delete';

@Component({
  selector: 'app-list-bank-account',
  templateUrl: './list.page.html',
  imports: [
    ButtonComponent,
    NgIcon,
    ItemActionsComponent,
    PaginatorComponent,
    CurrencyPipe,
    FormsModule,
    ReactiveFormsModule,
    EmptyListComponent,
    CardSkeletonListComponent,
    NgClass,
    RouterOutlet,
  ],
  providers: [
    provideIcons({
      heroFunnel,
      heroTrash,
      heroPencilSquare,
      heroPlus,
      heroWallet,
      heroBanknotes,
      heroDevicePhoneMobile,
      heroBuildingLibrary,
    }),
  ],
})
export class ListPage implements OnInit {
  readonly BANK_ACCOUNT_LIMIT_OPTIONS = BANK_ACCOUNT_LIMIT_OPTIONS;
  readonly eBankAccountType = eBankAccountType;
  readonly BankAccountTypeInfo = BankAccountTypeInfo;

  readonly bankAccountActions: iItemActionData<tBankAccountAction>[] = [
    {
      key: 'edit',
      label: 'Editar',
      icon: 'heroPencilSquare',
    },
    {
      key: 'delete',
      label: 'Excluir',
      icon: 'heroTrash',
      hoverClass: 'hover:bg-error/10 hover:text-error',
    },
  ];

  readonly isMobile = inject(BreakpointObserverService).isMobile;

  readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly bankAccountApi = inject(BankAccountApi);
  private readonly refreshService = inject(RefreshService);
  private readonly confirmationModalService = inject(ConfirmationModalService);
  private readonly toastService = inject(ToastService);
  private readonly apiErrorHandlerService = inject(ApiErrorHandlerService);

  readonly searchControl = new FormControl('', { nonNullable: true });

  private readonly bankAccountList = createListResource<iBankAccountListFilters, iBankAccount>({
    parser: bankAccountListQueryParser,
    fetch: ({ offset, limit, filters }) =>
      this.bankAccountApi.getAll(offset, limit, filters.bankAccountName),
    refresh$: this.refreshService.on(REFRESH_KEYS.BANK_ACCOUNTS),
  });
  readonly response = this.bankAccountList.response;
  readonly onOffsetChange = this.bankAccountList.setOffset;
  readonly onLimitChange = this.bankAccountList.setLimit;
  readonly isLoading = this.bankAccountList.isLoading;

  constructor() {
    effect(() => {
      const bankAccountName = this.bankAccountList.filters().bankAccountName ?? '';

      if (this.searchControl.value === bankAccountName) return;

      this.searchControl.setValue(bankAccountName, { emitEvent: false });
    });
  }

  ngOnInit(): void {
    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(value => {
        this.bankAccountList.setFilters({
          bankAccountName: value?.trim() || undefined,
        });
      });
  }

  async onBankAccountAction(event: iItemActionEvent<tBankAccountAction, iBankAccount>) {
    switch (event.action) {
      case 'edit':
        this.editBankAccount(event.item);
        break;

      case 'delete':
        await this.deleteBankAccount(event.item);
        break;
    }
  }

  private editBankAccount(bankAccount: iBankAccount): void {
    this.router.navigate([bankAccount.id], {
      relativeTo: this.route,
      queryParamsHandling: 'preserve',
    });
  }
  private async deleteBankAccount(bankAccount: iBankAccount) {
    const confirm = await this.confirmationModalService.show({
      title: 'Excluir conta bancária',
      message: `Deseja realmente excluir "${bankAccount.name}"? Essa ação não poderá ser desfeita.`,
      type: 'danger',
    });

    if (!confirm) return;

    this.bankAccountApi.delete(bankAccount.id).subscribe({
      next: () => {
        this.toastService.show(
          'success',
          'Conta bancária excluída',
          'Sua conta bancária foi excluída com sucesso.',
        );
        this.refreshService.trigger(REFRESH_KEYS.BANK_ACCOUNTS);
      },
      error: (error: iErrorResponse) => {
        this.apiErrorHandlerService.show(
          error,
          'Não foi possível excluir a conta bancária',
          'Tivemos um problema ao excluir sua conta bancária. Tente novamente em alguns instantes.',
        );
      },
    });
  }

  onCreateBankAccount(): void {
    this.router.navigate(['nova'], { relativeTo: this.route, queryParamsHandling: 'preserve' });
  }
}
