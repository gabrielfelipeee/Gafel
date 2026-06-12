import { inject, Injectable } from '@angular/core';
import { ToastService } from './toast.service';
import { iErrorResponse } from '@shared/interfaces/error-response.interface';

@Injectable({ providedIn: 'root' })
export class ApiErrorHandlerService {
  private readonly toastService = inject(ToastService);

  show(error: iErrorResponse, fallbackTitle: string, fallbackMessage: string): void {
    const title =
      error.error?.status !== 500 && error.error?.title ? error.error?.title : fallbackTitle;

    const message =
      error.error?.status !== 500 && error.error?.detail ? error.error.detail : fallbackMessage;

    this.toastService.show('error', title, message);
  }
}
