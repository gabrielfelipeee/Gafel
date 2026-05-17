import { Component, computed, Injectable, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { toast } from 'ngx-sonner';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  heroCheckCircle,
  heroExclamationCircle,
  heroExclamationTriangle,
  heroXCircle,
} from '@ng-icons/heroicons/outline';

type tToastType = 'success' | 'info' | 'warning' | 'error';

interface iToast {
  type: tToastType;
  title: string;
  description?: string;
}

@Component({
  selector: 'app-custom-toast',
  standalone: true,
  imports: [NgIcon, NgClass],
  providers: [
    provideIcons({
      heroCheckCircle,
      heroExclamationCircle,
      heroExclamationTriangle,
      heroXCircle,
    }),
  ],
  template: `<div class="toast-custom flex items-start rounded-xl p-4 gap-4 text-success">
    <div
      class="h-8 w-8 flex items-center justify-center rounded-full"
      [ngClass]="{
        'text-success bg-success/10': type() === 'success',
        'text-info bg-info/10': type() === 'info',
        'text-warning bg-warning/10': type() === 'warning',
        'text-error bg-error/10': type() === 'error',
      }"
    >
      <ng-icon [name]="config().icon" size="26" />
    </div>

    <div class="flex-1 grid gap-1">
      <span
        class="font-mono text-[12px] tracking-widest uppercase"
        [class.text-success]="type() === 'success'"
        [class.text-info]="type() === 'info'"
        [class.text-warning]="type() === 'warning'"
        [class.text-error]="type() === 'error'"
        >{{ config().label }}</span
      >

      <span class="font-semibold text-base font-display"> {{ toast().title }} </span>

      @if (toast().description) {
        <span class="text-[#626F84]"> {{ toast().description }} </span>
      }
    </div>
  </div>`,
})
class CustomToastComponent {
  private readonly toastConfig = {
    success: {
      icon: 'heroCheckCircle',
      label: 'sucesso',
    },
    info: {
      icon: 'heroExclamationCircle',
      label: 'informação',
    },
    warning: {
      icon: 'heroExclamationTriangle',
      label: 'atenção',
    },
    error: {
      icon: 'heroXCircle',
      label: 'erro',
    },
  } as const;

  protected readonly toast = input.required<iToast>();

  type = computed(() => this.toast().type);
  config = computed(() => this.toastConfig[this.type()]);
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private readonly DURATION = 5000 as const;

  show(type: tToastType, title: string, description?: string) {
    toast('', {
      component: CustomToastComponent,
      componentProps: {
        toast: {
          type,
          title,
          description,
        } as iToast,
      },
      duration: this.DURATION,
    });
  }
}
