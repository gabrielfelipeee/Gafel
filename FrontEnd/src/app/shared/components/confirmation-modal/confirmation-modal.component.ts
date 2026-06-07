import { Component, inject } from '@angular/core';
import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  heroCheckCircle,
  heroExclamationTriangle,
  heroInformationCircle,
  heroPlus,
  heroTrash,
  heroXMark,
} from '@ng-icons/heroicons/outline';
import { IconButtonComponent } from '../icon-button/icon-button.component';
import { iConfirmationDialogData } from '@shared/interfaces/confirmation-dialog-data.interface';

@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  imports: [IconButtonComponent, NgIcon],
  providers: [
    provideIcons({
      heroXMark,
      heroInformationCircle,
      heroExclamationTriangle,
      heroPlus,
      heroTrash,
      heroCheckCircle,
    }),
  ],
})
export class ConfirmationModalComponent {
  readonly data = inject<iConfirmationDialogData>(DIALOG_DATA);
  private dialogRef = inject<DialogRef<boolean>>(DialogRef<boolean>);

  private readonly configs = {
    danger: {
      icon: 'heroTrash',
      iconClass: 'bg-error/15 text-error',
      buttonClass: 'btn-error',
      defaultText: 'Excluir',
    },

    warning: {
      icon: 'heroExclamationTriangle',
      iconClass: 'bg-warning/15 text-warning',
      buttonClass: 'btn-warning',
      defaultText: 'Continuar',
    },

    success: {
      icon: 'heroCheckCircle',
      iconClass: 'bg-success/15 text-success',
      buttonClass: 'btn-success',
      defaultText: 'Ok',
    },

    info: {
      icon: 'heroInformationCircle',
      iconClass: 'bg-info/15 text-info',
      buttonClass: 'btn-info',
      defaultText: 'Ok',
    },
  } as const;
  readonly config = this.configs[this.data.type];

  close(confirm: boolean) {
    this.dialogRef.close(confirm);
  }
}
