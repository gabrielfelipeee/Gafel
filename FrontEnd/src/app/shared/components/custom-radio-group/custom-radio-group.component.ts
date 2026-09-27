import { Component, input, output } from '@angular/core';
import { NgClass } from '@angular/common';
import { NgIcon } from '@ng-icons/core';
import { CustomRadioGroupOption } from '@shared/interfaces/custom-radio-group-option.interface';

@Component({
  host: { class: 'block' },
  selector: 'app-custom-radio-group',
  templateUrl: './custom-radio-group.component.html',
  imports: [NgClass, NgIcon],
})
export class CustomRadioGroupComponent<T> {
  options = input.required<CustomRadioGroupOption<T>[]>();
  value = input.required<T>();
  disabled = input<boolean>(false);

  valueChange = output<T>();

  onClick(value: T) {
    this.valueChange.emit(value);
  }
}
