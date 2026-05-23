import { Component, input, output } from '@angular/core';
import { NgClass } from '@angular/common';
import { NgIcon } from '@ng-icons/core';
import { iCustomRadioOption } from '@shared/interfaces/custom-radio-option.interface';

@Component({
  selector: 'app-custom-radio-group',
  templateUrl: './custom-radio-group.component.html',
  imports: [NgClass, NgIcon],
})
export class CustomRadioGroupComponent<T> {
  options = input.required<iCustomRadioOption<T>[]>();
  value = input.required<T>();
  valueChange = output<T>();

  onClick(value: T) {
    this.valueChange.emit(value);
  }
}
