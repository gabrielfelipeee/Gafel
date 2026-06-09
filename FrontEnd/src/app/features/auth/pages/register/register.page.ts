import { Component, inject } from '@angular/core';
import { FormGroup, NonNullableFormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  heroArrowRight,
  heroUserPlus,
  heroChartPie,
  heroPencilSquare,
  heroBolt,
  heroEnvelope,
  heroLockClosed,
  heroShieldCheck,
  heroUser,
} from '@ng-icons/heroicons/outline';
import { AuthLayout } from '@core/layouts/layouts/auth/auth.layout';
import { FULLNAME_VALIDATION_MESSAGES } from '@shared/validation/messages/fullname-validation.messages';
import { EMAIL_VALIDATION_MESSAGES } from '@shared/validation/messages/email-validation.messages';
import { PASSWORD_VALIDATION_MESSAGES } from '@shared/validation/messages/password-validation.messages';
import { PasswordValidators } from '@shared/validation/validators/password.validators';
import { tFormValidationMessages } from '@shared/types/form-validation-messages.type';
import {
  EMAIL_RULES,
  FULLNAME_RULES,
  PASSWORD_RULES,
} from '@shared/validation/rules/auth-validation.rules';
import { CustomInputComponent } from '@shared/components/custom-input/custom-input.component';
import { iFeatureCard } from '../../interfaces/feature-card.interface';
import { iRegisterForm } from '../../interfaces/register-form.interface';
import { AuthButtonComponent } from '../../components/auth-button/auth-button.component';
import { RegisterUserFacade } from '@core/auth/facades/register-user.facade';
import { iRegisterUserRequest } from '@core/auth/interfaces/register-user-request.interface';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { Router, RouterLink } from '@angular/router';
import { applyApiValidationErrors } from '@shared/validation/utils/apply-api-validation.utils';

@Component({
  selector: 'app-register-page',
  templateUrl: './register.page.html',
  imports: [
    AuthLayout,
    NgIcon,
    ReactiveFormsModule,
    AuthButtonComponent,
    CustomInputComponent,
    RouterLink,
  ],
  viewProviders: [
    provideIcons({
      heroUserPlus,
      heroUser,
      heroArrowRight,
      heroChartPie,
      heroPencilSquare,
      heroBolt,
      heroEnvelope,
      heroLockClosed,
      heroShieldCheck,
    }),
  ],
})
export class RegisterPage {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly registerUserFacade = inject(RegisterUserFacade);
  private readonly router = inject(Router);

  protected readonly formErrorMessages = {
    fullName: FULLNAME_VALIDATION_MESSAGES,
    email: EMAIL_VALIDATION_MESSAGES,
    password: PASSWORD_VALIDATION_MESSAGES,
    confirmPassword: {
      required: 'Confirmação de senha é obrigatória',
      passwordMismatch: 'As senhas não coincidem',
    },
  } satisfies tFormValidationMessages;

  protected readonly features: iFeatureCard[] = [
    {
      title: 'Visão completa',
      description: 'Acompanhe gastos e resultados em tempo real.',
      icon: 'heroChartPie',
    },
    {
      title: 'Metas inteligentes',
      description: 'Defina objetivos e acompanhe sua evolução.',
      icon: 'heroPencilSquare',
    },
    {
      title: 'Rápido e gratuito',
      description: 'Comece em minutos, sem mensalidades.',
      icon: 'heroBolt',
    },
  ];

  form: FormGroup<iRegisterForm> = this.formBuilder.group(
    {
      fullName: [
        '',
        [
          Validators.required,
          Validators.minLength(FULLNAME_RULES.MIN_LENGTH),
          Validators.maxLength(FULLNAME_RULES.MAX_LENGTH),
        ],
      ],
      email: [
        '',
        [Validators.required, Validators.email, Validators.maxLength(EMAIL_RULES.MAX_LENGTH)],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(PASSWORD_RULES.MIN_LENGTH),
          PasswordValidators.containsDigit(),
          PasswordValidators.containsSpecialCharacter(),
        ],
      ],
      confirmPassword: ['', [Validators.required]],
    },
    {
      validators: PasswordValidators.matchPassword('password', 'confirmPassword'),
    },
  );

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request: iRegisterUserRequest = this.form.getRawValue();

    this.registerUserFacade.register(request).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (error: iErrorResponse) => applyApiValidationErrors(this.form, error),
    });
  }
}
