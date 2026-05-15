import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { iRegisterUserRequest } from '../interfaces/register-user-request.interface';
import { Observable } from 'rxjs';
import { iAuthResponse } from '../interfaces/auth-response.interface';
import { environment } from '../../../../environments/environment.development';
import { iUserCredentials } from '../interfaces/user-credentials.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthApi {
  private readonly baseUrl = `${environment.apiUrl}/auth`;

  private readonly http = inject(HttpClient);

  register(user: iRegisterUserRequest): Observable<iAuthResponse> {
    return this.http.post<iAuthResponse>(`${this.baseUrl}/register`, user);
  }

  login(credentials: iUserCredentials): Observable<iAuthResponse> {
    return this.http.post<iAuthResponse>(`${this.baseUrl}/login`, credentials);
  }
}
