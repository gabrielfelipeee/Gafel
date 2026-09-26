import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { iPagedResponse } from '@shared/interfaces/paged-response.interface';
import { BankAccount } from '../interfaces/bank-account.interface';
import { iCreateOrEditBankAccountRequest } from '../interfaces/create-or-edit-bank-account-request.interface';

@Injectable({
  providedIn: 'root',
})
export class BankAccountApi {
  private readonly baseUrl = `${environment.apiUrl}/bankaccounts`;

  private readonly http = inject(HttpClient);

  create(bankAccount: iCreateOrEditBankAccountRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, bankAccount);
  }

  update(id: string, bankAccount: iCreateOrEditBankAccountRequest) {
    return this.http.put<void>(`${this.baseUrl}/${id}`, bankAccount);
  }

  getAll(
    offset = 0,
    limit = 10,
    bankAccountName: string | null = null,
  ): Observable<iPagedResponse<BankAccount>> {
    let params = new HttpParams().set('offset', offset).set('limit', limit);

    if (bankAccountName) params = params.set('bankAccountName', bankAccountName);

    return this.http.get<iPagedResponse<BankAccount>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<BankAccount> {
    return this.http.get<BankAccount>(`${this.baseUrl}/${id}`);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
