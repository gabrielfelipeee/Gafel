import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { provideIcons } from '@ng-icons/core';
import { faSolidBrazilianRealSign } from '@ng-icons/font-awesome/solid';
import { ModalComponent } from '@shared/components/modal/modal.component';
import { CustomInputComponent } from '@shared/components/custom-input/custom-input.component';
import { ButtonComponent } from '@shared/components/button/button.component';
import { tFormValidationMessages } from '@shared/types/form-validation-messages.type';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { mapApiErrorsToForm } from '@shared/validation/utils/map-api-errors-to-form.utils';
import { RefreshService } from '@shared/services/refresh.service';
import { REFRESH_KEYS } from '@shared/constants/refresh-keys.constant';
import { FormValidationService } from '@shared/services/form-validation.service';
import { ApiErrorHandlerService } from '@shared/services/api-error-handler.service';
import { iBankAccount } from '@features/bank-account/interfaces/bank-account.interface';
import { iCreateOrEditBankAccountForm } from '@features/bank-account/interfaces/create-or-edit-bank-account-form.interface';
import { BANK_ACCOUNT_RULES } from '@features/bank-account/rules/bank-account.rules';
import {
  BankAccountTypeInfo,
  eBankAccountType,
} from '@features/bank-account/enums/bank-account-type.enum';
import { iCreateOrEditBankAccountRequest } from '@features/bank-account/interfaces/create-or-edit-bank-account-request.interface';
import { BankAccountApi } from '@features/bank-account/apis/bank-account.api';
import { CustomSelectComponent } from '@shared/components/custom-select/custom-select.component';
import { SelectOption } from '@shared/interfaces/select-option.interface';
import { heroArrowPath } from '@ng-icons/heroicons/outline';

@Component({
  selector: 'app-create-or-edit-bank-account',
  templateUrl: './create-or-edit.page.html',
  imports: [
    ReactiveFormsModule,
    ModalComponent,
    CustomInputComponent,
    ButtonComponent,
    CustomSelectComponent,
  ],
  providers: [
    provideIcons({
      faSolidBrazilianRealSign,
      heroArrowPath,
    }),
  ],
})
export class CreateOrEditPage {
  readonly BankAccountTypeInfo = BankAccountTypeInfo;

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly toastService = inject(ToastService);
  private readonly bankAccountApi = inject(BankAccountApi);
  private readonly refreshService = inject(RefreshService);
  private readonly formValidationService = inject(FormValidationService);
  private readonly apiErrorHandlerService = inject(ApiErrorHandlerService);

  readonly bankAccount: iBankAccount | undefined = this.route.snapshot.data['bankAccount'];

  readonly bankAccountTypeOptions: SelectOption[] = Object.values(eBankAccountType)
    .filter((v): v is eBankAccountType => typeof v === 'number')
    .map(value => ({
      value,
      label: BankAccountTypeInfo[value].label,
    }));
  readonly formErrorMessages = {
    name: {
      required: 'Nome é obrigatório',
      maxlength: `Nome deve ter no máximo ${BANK_ACCOUNT_RULES.NAME.MAX_LENGTH} caracteres`,
    },
    initialBalance: {
      required: 'Saldo inicial é obrigatório',
      min: 'O valor deve ser maior que R$ 0,00',
    },
    type: {
      required: 'Tipo é obrigatório',
    },
  } satisfies tFormValidationMessages;

  readonly form: FormGroup<iCreateOrEditBankAccountForm> = this.formBuilder.group({
    name: [
      this.bankAccount?.name ?? '',
      [Validators.required, Validators.maxLength(BANK_ACCOUNT_RULES.NAME.MAX_LENGTH)],
    ],
    initialBalance: [
      this.bankAccount?.initialBalance ?? 0,
      [Validators.required, Validators.min(BANK_ACCOUNT_RULES.INITIAL_BALANCE.MIN)],
    ],
    type: [this.bankAccount?.type ?? eBankAccountType.CheckingAccount, [Validators.required]],
  });
  readonly selectedBankAccountType = toSignal(this.form.controls.type.valueChanges, {
    initialValue: this.form.controls.type.value,
  });

  onSubmit() {
    if (!this.formValidationService.validate(this.form)) return;

    this.save(this.form.getRawValue());
  }
  private save(bankAccount: iCreateOrEditBankAccountRequest) {
    const request$ = this.bankAccount
      ? this.bankAccountApi.update(this.bankAccount.id, bankAccount)
      : this.bankAccountApi.create(bankAccount);

    request$.subscribe({
      next: () => {
        const title = this.bankAccount ? 'Conta bancária atualizada' : 'Conta bancária criada';
        const message = this.bankAccount
          ? 'As alterações foram salvas com sucesso.'
          : 'Sua conta bancária foi criada com sucesso.';

        this.toastService.show('success', title, message);

        this.refreshService.trigger(REFRESH_KEYS.BANK_ACCOUNTS);
        this.router.navigate(['/contas'], { queryParamsHandling: 'preserve' });
      },
      error: (error: iErrorResponse) => {
        mapApiErrorsToForm(this.form, error);

        this.apiErrorHandlerService.show(
          error,
          'Não foi possível criar a conta bancária',
          'Tivemos um problema ao criar sua conta bancária. Tente novamente em alguns instantes.',
        );
      },
    });
  }

  onModalClose(): void {
    this.router.navigate(['/contas'], { queryParamsHandling: 'preserve' });
  }
}
