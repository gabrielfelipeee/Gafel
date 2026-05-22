import { Component, inject, OnInit, signal } from '@angular/core';
import { NgClass } from '@angular/common';
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

@Component({
  selector: 'app-create-or-edit-category',
  templateUrl: './create-or-edit.page.html',
  imports: [
    ReactiveFormsModule,
    ModalComponent,
    NgIcon,
    NgClass,
    CustomInputComponent,
    ButtonComponent,
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
export class CreateOrEditPage implements OnInit {
  eCategoryType = eCategoryType;
  icons = icons;
  private readonly router = inject(Router);
  private readonly formBuilder = inject(NonNullableFormBuilder);

  protected readonly formErrorMessages = {
    name: {
      required: 'Nome é obrigatório',
      maxlength: `Nome deve ter no máximo ${CATEGORY_RULES.NAME.MAX_LENGTH} caracteres`,
    },
  } satisfies tFormValidationMessages;

  readonly categories = [
    {
      type: eCategoryType.Income,
      label: 'receita',
      icon: 'heroArrowUp',
    },
    {
      type: eCategoryType.Expense,
      label: 'despesa',
      icon: 'heroArrowDown',
    },
  ];

  form!: FormGroup<iCreateOrEditCategoryForm>;
  selectedCategory = signal<eCategoryType>(eCategoryType.Income);
  selectedIcon = signal<string>('');

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(CATEGORY_RULES.NAME.MAX_LENGTH)]],
      icon: ['', [Validators.required]],
      type: [eCategoryType.Income, [Validators.required]],
    });

    this.form.controls.type.valueChanges.subscribe(value => this.selectedCategory.set(value));
    this.form.controls.icon.valueChanges.subscribe(value => this.selectedIcon.set(value));
  }

  onSubmit() {
    console.log(this.form.value);
  }

  onSelectCategory(type: eCategoryType): void {
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
