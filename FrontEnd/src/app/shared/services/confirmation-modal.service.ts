import { inject, Injectable } from '@angular/core';
import { Dialog } from '@angular/cdk/dialog';
import { ConfirmationModalComponent } from '@shared/components/confirmation-modal/confirmation-modal.component';
import { firstValueFrom } from 'rxjs';
import { iConfirmationDialogData } from '@shared/interfaces/confirmation-dialog-data.interface';

@Injectable({
  providedIn: 'root',
})
export class ConfirmationModalService {
  private readonly dialog = inject(Dialog);

  async show(data: iConfirmationDialogData): Promise<boolean> {
    const dialogRef = this.dialog.open<boolean>(ConfirmationModalComponent, {
      data,
    });

    const result = await firstValueFrom(dialogRef.closed);
    return !!result;
  }
}
