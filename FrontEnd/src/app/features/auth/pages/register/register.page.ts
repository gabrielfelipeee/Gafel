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
import { PasswordValidators } from '@shared/validators/password.validators';
import { CustomInputComponent } from '@shared/components/custom-input/custom-input.component';
import { iFeatureCard } from '../../interfaces/feature-card.interface';
import { iRegisterForm } from '../../interfaces/register-form.interface';
import { AuthButtonComponent } from '../../components/auth-button/auth-button.component';
import { RegisterUserFacade } from '@core/auth/facades/register-user.facade';
import { iRegisterUserRequest } from '@core/auth/interfaces/register-user-request.interface';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { Router, RouterLink } from '@angular/router';
import { mapApiErrorsToForm } from '@shared/utils/map-api-errors-to-form.utils';
import { FormValidationService } from '@shared/services/form-validation.service';
import { ApiErrorHandlerService } from '@shared/services/api-error-handler.service';
import { FormErrorMessages } from '@shared/types/form-validation.types';
import { USER_VALIDATION_MESSAGES } from '@shared/messages/user-validation.messages';
import { USER_RULES } from '@shared/rules/user.rules';

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
  private readonly formValidationService = inject(FormValidationService);
  private readonly apiErrorHandlerService = inject(ApiErrorHandlerService);

  protected readonly formErrorMessages: FormErrorMessages<iRegisterForm> = {
    fullName: USER_VALIDATION_MESSAGES.FULL_NAME,
    email: USER_VALIDATION_MESSAGES.EMAIL,
    password: USER_VALIDATION_MESSAGES.PASSWORD,
    confirmPassword: {
      required: 'Confirmação de senha é obrigatória',
      passwordMismatch: 'As senhas não coincidem',
    },
  };

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
          Validators.minLength(USER_RULES.FULL_NAME.MIN_LENGTH),
          Validators.maxLength(USER_RULES.FULL_NAME.MAX_LENGTH),
        ],
      ],
      email: [
        '',
        [Validators.required, Validators.email, Validators.maxLength(USER_RULES.EMAIL.MAX_LENGTH)],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(USER_RULES.PASSWORD.MIN_LENGTH),
          Validators.maxLength(USER_RULES.PASSWORD.MAX_LENGTH),
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
    if (!this.formValidationService.validate(this.form)) return;

    const request: iRegisterUserRequest = this.form.getRawValue();

    this.registerUserFacade.register(request).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (error: iErrorResponse) => {
        mapApiErrorsToForm(this.form, error);

        this.apiErrorHandlerService.show(
          error,
          'Não foi possível criar sua conta',
          'Tivemos um problema ao concluir seu cadastro. Tente novamente em alguns instantes.',
        );
      },
    });
  }
}
