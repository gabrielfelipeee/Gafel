import { NgTemplateOutlet } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { provideIcons } from '@ng-icons/core';
import { heroArrowLeft, heroArrowPath, heroHashtag } from '@ng-icons/heroicons/outline';
import { ModalComponent } from '@shared/components/modal/modal.component';
import { CategoryType } from '@features/category/enums/category-type.enum';
import { iCreateOrEditCategoryForm } from '@features/category/interfaces/create-or-edit-category-form.interface';
import { CustomInputComponent } from '@shared/components/custom-input/custom-input.component';
import { ButtonComponent } from '@shared/components/button/button.component';
import { FormValidationMessages } from '@shared/types/form-validation-messages.type';
import { CATEGORY_RULES } from '@features/category/rules/category.rules';
import { CategoryApi } from '@features/category/apis/category.api';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { mapApiErrorsToForm } from '@shared/validation/utils/map-api-errors-to-form.utils';
import { CustomRadioGroupComponent } from '@shared/components/custom-radio-group/custom-radio-group.component';
import { RefreshService } from '@shared/services/refresh.service';
import { REFRESH_KEYS } from '@shared/constants/refresh-keys.constant';
import { Category } from '@features/category/interfaces/category.interface';
import { iCreateOrEditCategoryRequest } from '@features/category/interfaces/create-or-edit-category-request.interface';
import { FormValidationService } from '@shared/services/form-validation.service';
import { ApiErrorHandlerService } from '@shared/services/api-error-handler.service';
import { BreakpointObserverService } from '@shared/services/breakpoint-observer.service';
import { IconButtonComponent } from '@shared/components/icon-button/icon-button.component';
import { finalize } from 'rxjs';
import { CustomRadioGroupOption } from '@shared/interfaces/custom-radio-group-option.interface';
import { EmojiComponent } from '@shared/components/emoji/emoji.component';
import { CATEGORY_ICON_NAMES } from '@features/category/contants/category-icon-names.constant';

@Component({
  selector: 'app-create-or-edit-category',
  templateUrl: './create-or-edit.page.html',
  imports: [
    ReactiveFormsModule,
    ModalComponent,
    CustomInputComponent,
    ButtonComponent,
    CustomRadioGroupComponent,
    NgTemplateOutlet,
    IconButtonComponent,
    EmojiComponent,
  ],
  providers: [provideIcons({ heroHashtag, heroArrowPath, heroArrowLeft })],
})
export class CreateOrEditPage {
  readonly CATEGORY_ICON_NAMES = CATEGORY_ICON_NAMES;

  readonly formErrorMessages = {
    name: {
      required: 'Nome é obrigatório',
      maxlength: `Nome deve ter no máximo ${CATEGORY_RULES.NAME.MAX_LENGTH} caracteres`,
    },
    icon: {
      required: 'Ícone é obrigatório',
    },
  } satisfies FormValidationMessages;

  readonly categoryTypeOptions: CustomRadioGroupOption<CategoryType>[] = [
    {
      value: CategoryType.Income,
      label: 'receita',
      icon: 'heroArrowUp',
      variant: 'success',
    },
    {
      value: CategoryType.Expense,
      label: 'despesa',
      icon: 'heroArrowDown',
      variant: 'error',
    },
  ];

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly toastService = inject(ToastService);
  private readonly categoryApi = inject(CategoryApi);
  private readonly refreshService = inject(RefreshService);
  private readonly formValidationService = inject(FormValidationService);
  private readonly apiErrorHandlerService = inject(ApiErrorHandlerService);
  readonly isMobile = inject(BreakpointObserverService).isMobile;

  readonly category: Category | undefined = this.route.snapshot.data['category'];

  readonly form: FormGroup<iCreateOrEditCategoryForm> = this.formBuilder.group({
    name: [
      this.category?.name ?? '',
      [Validators.required, Validators.maxLength(CATEGORY_RULES.NAME.MAX_LENGTH)],
    ],
    icon: [this.category?.icon ?? '', [Validators.required]],
    type: [this.category?.type ?? CategoryType.Income, [Validators.required]],
  });

  onSelectCategoryType(type: CategoryType): void {
    this.form.controls.type.setValue(type);
  }
  onSelectIcon(icon: string): void {
    this.form.controls.icon.setValue(icon);
  }

  onSubmit() {
    if (this.form.disabled) return;

    if (!this.formValidationService.validate(this.form)) return;

    this.save(this.form.getRawValue());
  }
  teste = 1;
  private save(category: iCreateOrEditCategoryRequest) {
    this.form.disable();

    const request$ = this.category
      ? this.categoryApi.update(this.category.id, category)
      : this.categoryApi.create(category);

    request$.pipe(finalize(() => this.form.enable())).subscribe({
      next: () => {
        const title = this.category ? 'Categoria atualizada' : 'Categoria criada';
        const message = this.category
          ? 'As alterações foram salvas com sucesso.'
          : 'Sua categoria foi criada com sucesso e já está pronta para uso.';

        this.toastService.show('success', title, message);

        this.refreshService.trigger(REFRESH_KEYS.CATEGORIES);
        this.router.navigate(['/categorias'], { queryParamsHandling: 'preserve' });
      },
      error: (error: iErrorResponse) => {
        mapApiErrorsToForm(this.form, error);

        this.apiErrorHandlerService.show(
          error,
          'Não foi possível criar a categoria',
          'Tivemos um problema ao criar sua categoria. Tente novamente em alguns instantes.',
        );
      },
    });
  }

  onClose(): void {
    this.router.navigate(['/categorias'], { queryParamsHandling: 'preserve' });
  }
}
