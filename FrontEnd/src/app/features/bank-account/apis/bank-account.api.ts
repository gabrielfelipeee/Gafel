import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { iPagedResponse } from '@shared/interfaces/paged-response.interface';
import { iBankAccount } from '../interfaces/bank-account.interface';
import { eBankAccountType } from '../enums/bank-account-type.enum';

@Injectable({
  providedIn: 'root',
})
export class BankAccountApi {
  private readonly baseUrl = `${environment.apiUrl}/bankaccounts`;

  private readonly http = inject(HttpClient);

  getAll(
    offset = 0,
    limit = 10,
    type: eBankAccountType | null = null,
    bankAccountName: string | null = null,
  ): Observable<iPagedResponse<iBankAccount>> {
    let params = new HttpParams().set('offset', offset).set('limit', limit);

    if (type !== null && type !== undefined) params = params.set('type', type);
    if (bankAccountName) params = params.set('bankAccountName', bankAccountName);

    return this.http.get<iPagedResponse<iBankAccount>>(this.baseUrl, { params });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
