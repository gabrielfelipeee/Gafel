export interface iConfirmationDialogData {
  title: string;
  message: string;
  type: 'danger' | 'warning' | 'success' | 'info';

  cancelText?: string;
  confirmText?: string;
}
