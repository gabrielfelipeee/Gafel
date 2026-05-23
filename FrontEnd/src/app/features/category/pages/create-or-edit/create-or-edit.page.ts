import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  heroArrowDown,
  heroArrowUp,
  heroHashtag,
  heroPlus,
  heroXMark,
  heroAcademicCap,
  heroArchiveBox,
  heroArrowTrendingDown,
  heroArrowTrendingUp,
  heroBanknotes,
  heroBeaker,
  heroBolt,
  heroBookOpen,
  heroBriefcase,
  heroBuildingOffice,
  heroCake,
  heroCalendar,
  heroCamera,
  heroChartBar,
  heroChartPie,
  heroClock,
  heroCreditCard,
  heroCurrencyDollar,
  heroEllipsisHorizontalCircle,
  heroFilm,
  heroFire,
  heroGift,
  heroGlobeAmericas,
  heroHeart,
  heroHome,
  heroLockClosed,
  heroMap,
  heroMusicalNote,
  heroPresentationChartLine,
  heroReceiptPercent,
  heroReceiptRefund,
  heroScale,
  heroShieldCheck,
  heroShoppingBag,
  heroShoppingCart,
  heroTruck,
  heroTv,
  heroWallet,
  heroWifi,
  heroTag,
} from '@ng-icons/heroicons/outline';
import { ModalComponent } from '@shared/components/modal/modal.component';
import { eCategoryType } from '@features/category/enums/category-type.enum';
import { iCreateOrEditCategoryForm } from '@features/category/interfaces/create-or-edit-category-form.interface';
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomInputComponent } from '@shared/components/custom-input/custom-input.component';
import { ButtonComponent } from '@shared/components/button/button.component';
import { tFormValidationMessages } from '@shared/types/form-validation-messages.type';
import { CATEGORY_RULES } from '@features/category/rules/category.rules';
import { icons } from '@features/category/data/icons.data';
import { CategoryApi } from '@features/category/apis/category.api';
import { ToastService } from '@shared/services/toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { applyApiValidationErrors } from '@shared/validation/utils/apply-api-validation.utils';
import { CustomRadioGroupComponent } from '@shared/components/custom-radio-group/custom-radio-group.component';
import { iCustomRadioOption } from '@shared/interfaces/custom-radio-option.interface';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-create-or-edit-category',
  templateUrl: './create-or-edit.page.html',
  imports: [
    ReactiveFormsModule,
    ModalComponent,
    NgIcon,
    CustomInputComponent,
    ButtonComponent,
    CustomRadioGroupComponent,
  ],
  providers: [
    provideIcons({
      heroPlus,
      heroXMark,
      heroArrowDown,
      heroArrowUp,
      heroHashtag,
      heroAcademicCap,
      heroArchiveBox,
      heroArrowTrendingDown,
      heroArrowTrendingUp,
      heroBanknotes,
      heroBeaker,
      heroBolt,
      heroBookOpen,
      heroBriefcase,
      heroBuildingOffice,
      heroCake,
      heroCalendar,
      heroCamera,
      heroChartBar,
      heroChartPie,
      heroClock,
      heroCreditCard,
      heroCurrencyDollar,
      heroEllipsisHorizontalCircle,
      heroFilm,
      heroFire,
      heroGift,
      heroGlobeAmericas,
      heroHeart,
      heroHome,
      heroLockClosed,
      heroMap,
      heroMusicalNote,
      heroPresentationChartLine,
      heroReceiptPercent,
      heroReceiptRefund,
      heroScale,
      heroShieldCheck,
      heroShoppingBag,
      heroShoppingCart,
      heroTruck,
      heroTv,
      heroWallet,
      heroWifi,
      heroTag,
    }),
  ],
})
export class CreateOrEditPage {
  readonly eCategoryType = eCategoryType;
  readonly icons = icons;

  private readonly router = inject(Router);
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly toastService = inject(ToastService);
  private readonly categoryApi = inject(CategoryApi);

  readonly formErrorMessages = {
    name: {
      required: 'Nome é obrigatório',
      maxlength: `Nome deve ter no máximo ${CATEGORY_RULES.NAME.MAX_LENGTH} caracteres`,
    },
    icon: {
      required: 'Ícone é obrigatório',
    },
  } satisfies tFormValidationMessages;

  readonly categoryTypeOptions: iCustomRadioOption<eCategoryType>[] = [
    {
      value: eCategoryType.Income,
      label: 'receita',
      icon: 'heroArrowUp',
    },
    {
      value: eCategoryType.Expense,
      label: 'despesa',
      icon: 'heroArrowDown',
      activeClass: 'border-transparent bg-error text-error-content',
      hoverClass: 'hover:border-error hover:text-error',
    },
  ];

  readonly form: FormGroup<iCreateOrEditCategoryForm> = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(CATEGORY_RULES.NAME.MAX_LENGTH)]],
    icon: ['', [Validators.required]],
    type: [eCategoryType.Income, [Validators.required]],
  });

  readonly selectedIcon = toSignal(this.form.controls.icon.valueChanges, {
    initialValue: this.form.controls.icon.value,
  });

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();

      this.toastService.show(
        'warning',
        'Ainda faltam informações',
        'Verifique os campos destacados.',
      );

      return;
    }

    this.categoryApi.create(this.form.getRawValue()).subscribe({
      next: () => {
        this.toastService.show('success', 'Categoria adicionada com sucesso');
        this.router.navigate(['/categorias']);
      },
      error: (error: iErrorResponse) => applyApiValidationErrors(this.form, error),
    });
  }

  onSelectCategoryType(type: eCategoryType): void {
    const control = this.form.controls.type;

    if (control.value === type) return;

    control.setValue(type);
  }
  onSelectIcon(icon: string): void {
    const control = this.form.controls.icon;

    if (control.value === icon) return;

    control.setValue(icon);
  }

  onModalClose(): void {
    this.router.navigate(['/categorias']);
  }
}
