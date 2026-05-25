import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { iCreateOrEditCategoryRequest } from '../interfaces/create-or-edit-category-request.interface';
import { iCategory } from '../interfaces/category.interface';
import { iPagedResponse } from '@shared/interfaces/paged-response.interface';

@Injectable({
  providedIn: 'root',
})
export class CategoryApi {
  private readonly baseUrl = `${environment.apiUrl}/categories`;

  private readonly http = inject(HttpClient);

  create(category: iCreateOrEditCategoryRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, category);
  }

  getAll(offset = 0, limit = 10): Observable<iPagedResponse<iCategory>> {
    const params = new HttpParams().set('offset', offset).set('limit', limit);

    return this.http.get<iPagedResponse<iCategory>>(this.baseUrl, { params });
  }
}
