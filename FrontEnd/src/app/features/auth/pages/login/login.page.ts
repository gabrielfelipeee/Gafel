import { Component, inject } from '@angular/core';
import { FormGroup, NonNullableFormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  heroArrowRight,
  heroArrowRightOnRectangle,
  heroBolt,
  heroChartBar,
  heroEnvelope,
  heroLockClosed,
  heroShieldCheck,
} from '@ng-icons/heroicons/outline';
import { AuthLayout } from '@core/layouts/layouts/auth/auth.layout';
import { PasswordValidators } from '@shared/validators/password.validators';
import { CustomInputComponent } from '@shared/components/custom-input/custom-input.component';
import { iFeatureCard } from '../../interfaces/feature-card.interface';
import { AuthButtonComponent } from '../../components/auth-button/auth-button.component';
import { iLoginForm } from '@features/auth/interfaces/login-form.interface';
import { iUserCredentials } from '@core/auth/interfaces/user-credentials.interface';
import { LoginFacade } from '@core/auth/facades/login.facade';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';
import { Router, RouterLink } from '@angular/router';
import { mapApiErrorsToForm } from '@shared/utils/map-api-errors-to-form.utils';
import { FormValidationService } from '@shared/services/form-validation.service';
import { ApiErrorHandlerService } from '@shared/services/api-error-handler.service';
import { FormErrorMessages } from '@shared/types/form-validation.types';
import { USER_VALIDATION_MESSAGES } from '@shared/messages/user-validation.messages';
import { USER_RULES } from '@shared/rules/user.rules';

@Component({
  selector: 'app-login-page',
  templateUrl: './login.page.html',
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
      heroArrowRightOnRectangle,
      heroArrowRight,
      heroLockClosed,
      heroEnvelope,
      heroChartBar,
      heroShieldCheck,
      heroBolt,
    }),
  ],
})
export class LoginPage {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly loginFacade = inject(LoginFacade);
  private readonly router = inject(Router);
  private readonly formValidationService = inject(FormValidationService);
  private readonly apiErrorHandlerService = inject(ApiErrorHandlerService);

  protected readonly formErrorMessages: FormErrorMessages<iLoginForm> = {
    email: USER_VALIDATION_MESSAGES.EMAIL,
    password: USER_VALIDATION_MESSAGES.PASSWORD,
  };

  protected readonly features: iFeatureCard[] = [
    {
      icon: 'heroChartBar',
      title: 'Acompanhe gastos e metas em tempo real',
    },
    {
      icon: 'heroShieldCheck',
      title: 'Seus dados protegidos com segurança',
    },
    {
      icon: 'heroBolt',
      title: 'Tudo disponível gratuitamente',
    },
  ];

  form: FormGroup<iLoginForm> = this.formBuilder.group({
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
  });

  onSubmit() {
    if (!this.formValidationService.validate(this.form)) return;

    const request: iUserCredentials = this.form.getRawValue();

    this.loginFacade.login(request).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (error: iErrorResponse) => {
        mapApiErrorsToForm(this.form, error);

        this.apiErrorHandlerService.show(
          error,
          'Não foi possível fazer login',
          'Tivemos um problema ao realizar seu login. Tente novamente em alguns instantes.',
        );
      },
    });
  }
}
