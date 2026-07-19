import { Component, input } from '@angular/core';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-logo',
  templateUrl: './logo.component.html',
  styleUrl: './logo.component.scss',
  imports: [NgClass],
})
export class LogoComponent {
  readonly compact = input(false);
}
