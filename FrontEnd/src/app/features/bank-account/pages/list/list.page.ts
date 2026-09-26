import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { provideIcons } from '@ng-icons/core';
import {
  heroBanknotes,
  heroBuildingLibrary,
  heroDevicePhoneMobile,
  heroFunnel,
  heroListBullet,
  heroPencil,
  heroPlus,
  heroSquares2x2,
  heroTrash,
  heroWallet,
} from '@ng-icons/heroicons/outline';
import { ButtonComponent } from '@shared/components/button/button.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginatorComponent } from '@shared/components/paginator/paginator.component';
import { iItemActionData } from '@shared/interfaces/item-action-data.interface';
import { createListResource } from '@shared/query/create-list-resource';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { EmptyListComponent } from '@shared/components/empty-list/empty-list.component';
import { RefreshService } from '@shared/services/refresh.service';
import { REFRESH_KEYS } from '@shared/constants/refresh-keys.constant';
import {
  BANK_ACCOUNT_LIMIT_OPTIONS,
  bankAccountListQueryParser,
} from '@features/bank-account/parsers/bank-account-list-query.parser';
import { iBankAccountListFilters } from '@features/bank-account/interfaces/bank-account-list-filters.interface';
import { BankAccount } from '@features/bank-account/interfaces/bank-account.interface';
import { BankAccountApi } from '@features/bank-account/apis/bank-account.api';
import { iItemActionEvent } from '@shared/interfaces/item-action-event.interface';
import { ConfirmationModalService } from '@shared/services/confirmation-modal.service';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { ApiErrorHandlerService } from '@shared/services/api-error-handler.service';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';
import { ContentSkeletonComponent } from '@shared/components/content-skeleton/content-skeleton.component';
import { IconButtonComponent } from '@shared/components/icon-button/icon-button.component';
import { ViewMode } from '@shared/types/view-mode.type';
import { BankAccountGridComponent } from './components/bank-account-grid/bank-account-grid.component';
import { BankAccountAction } from '@features/bank-account/types/bank-account-action.type';
import { BankAccountListComponent } from './components/bank-account-list/bank-account-list.component';

@Component({
  selector: 'app-list-bank-account',
  templateUrl: './list.page.html',
  imports: [
    ButtonComponent,
    PaginatorComponent,
    FormsModule,
    ReactiveFormsModule,
    EmptyListComponent,
    RouterOutlet,
    ContentSkeletonComponent,
    IconButtonComponent,
    BankAccountGridComponent,
    BankAccountListComponent,
  ],
  providers: [
    provideIcons({
      heroFunnel,
      heroTrash,
      heroPencil,
      heroPlus,
      heroWallet,
      heroBanknotes,
      heroDevicePhoneMobile,
      heroBuildingLibrary,
      heroListBullet,
      heroSquares2x2,
    }),
  ],
})
export class ListPage implements OnInit {
  readonly BANK_ACCOUNT_LIMIT_OPTIONS = BANK_ACCOUNT_LIMIT_OPTIONS;

  readonly actions: iItemActionData<BankAccountAction>[] = [
    {
      key: 'edit',
      label: 'Editar',
      icon: 'heroPencil',
    },
    {
      key: 'delete',
      label: 'Excluir',
      icon: 'heroTrash',
      hoverClass: 'hover:bg-error/10 hover:text-error',
    },
  ];

  private readonly router = inject(Router);
  private readonly bankAccountApi = inject(BankAccountApi);
  private readonly refreshService = inject(RefreshService);
  private readonly confirmationModalService = inject(ConfirmationModalService);
  private readonly toastService = inject(ToastService);
  private readonly apiErrorHandlerService = inject(ApiErrorHandlerService);
  readonly isMobile = inject(BreakpointObserverService).isMobile;
  readonly route = inject(ActivatedRoute);

  readonly searchControl = new FormControl('', { nonNullable: true });
  readonly viewMode = signal<ViewMode>('list');

  readonly selectedBankAccountIds = new Set<string>();

  private readonly bankAccountList = createListResource<iBankAccountListFilters, BankAccount>({
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

      if (this.searchControl.value !== bankAccountName)
        this.searchControl.setValue(bankAccountName, { emitEvent: false });
    });
    effect(() => {
      if (this.isMobile()) this.viewMode.set('grid');
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

  onBankAccountSelectionChange(result: { id: string; selected: boolean }): void {
    if (result?.selected) this.selectedBankAccountIds.add(result.id);
    else this.selectedBankAccountIds.delete(result.id);
  }
  onAllBankAccountsSelectionChange(selected: boolean): void {
    if (selected)
      this.response().items.forEach(bankAccount => this.selectedBankAccountIds.add(bankAccount.id));
    else this.selectedBankAccountIds.clear();
  }

  onViewModeChange(mode: ViewMode): void {
    this.viewMode.set(mode);
  }

  async handleBankAccountAction(event: iItemActionEvent<BankAccountAction, BankAccount>) {
    switch (event.action) {
      case 'edit':
        this.onCreateOrEditBankAccount(event.item);
        break;

      case 'delete':
        await this.deleteBankAccount(event.item);
        break;
    }
  }
  onCreateOrEditBankAccount(bankAccount?: BankAccount): void {
    this.router.navigate([bankAccount?.id ?? 'nova'], {
      relativeTo: this.route,
      queryParamsHandling: 'preserve',
    });
  }
  private async deleteBankAccount(bankAccount: BankAccount) {
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
}
