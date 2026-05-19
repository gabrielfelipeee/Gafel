import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main.layout.html',
  imports: [RouterOutlet, HeaderComponent],
})
export class MainLayout {}
