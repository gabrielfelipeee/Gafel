export type FormValidationErrorKey =
  // Angular
  | 'required'
  | 'min'
  | 'max'
  | 'minlength'
  | 'maxlength'
  | 'email'
  | 'pattern'

  // Customizados
  | 'requiresNumber'
  | 'requiresSpecialChar'
  | 'passwordMismatch';

export type FormFieldErrorMessages = Partial<Record<FormValidationErrorKey, string>>;

export type FormErrorMessages<T> = {
  [K in keyof T]?: FormFieldErrorMessages;
};
