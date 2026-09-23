import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { MobileNavigationComponent } from './components/mobile-navigation/mobile-navigation.component';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main.layout.html',
  imports: [RouterOutlet, HeaderComponent, MobileNavigationComponent],
})
export class MainLayout {}
