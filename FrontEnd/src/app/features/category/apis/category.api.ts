import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { iCreateOrEditCategoryRequest } from '../interfaces/create-or-edit-category-request.interface';
import { iCategory } from '../interfaces/category.interface';
import { iPagedResponse } from '@shared/interfaces/paged-response.interface';
import { eCategoryType } from '../enums/category-type.enum';

@Injectable({
  providedIn: 'root',
})
export class CategoryApi {
  private readonly baseUrl = `${environment.apiUrl}/categories`;

  private readonly http = inject(HttpClient);

  create(category: iCreateOrEditCategoryRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, category);
  }

  update(id: number, category: iCreateOrEditCategoryRequest) {
    return this.http.put<void>(`${this.baseUrl}/${id}`, category);
  }

  getAll(
    offset = 0,
    limit = 10,
    type: eCategoryType | null = null,
    categoryName: string | null = null,
  ): Observable<iPagedResponse<iCategory>> {
    let params = new HttpParams().set('offset', offset).set('limit', limit);

    if (type !== null && type !== undefined) params = params.set('type', type);
    if (categoryName) params = params.set('categoryName', categoryName);

    return this.http.get<iPagedResponse<iCategory>>(this.baseUrl, { params });
  }

  getById(id: number): Observable<iCategory> {
    return this.http.get<iCategory>(`${this.baseUrl}/${id}`);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
