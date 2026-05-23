import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { iCreateOrEditCategoryRequest } from '../interfaces/create-or-edit-category-request.interface';

@Injectable({
  providedIn: 'root',
})
export class CategoryApi {
  private readonly baseUrl = `${environment.apiUrl}/categories`;

  private readonly http = inject(HttpClient);

  create(category: iCreateOrEditCategoryRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, category);
  }
}
