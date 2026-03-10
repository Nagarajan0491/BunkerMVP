import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserInfo, LoginRequest, LoginResponse } from '../models';

const API_BASE = 'http://localhost:5005/api';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _user = signal<UserInfo | null>(null);
  readonly user = this._user.asReadonly();
  readonly isLoggedIn = () => this._user() !== null;

  constructor(private http: HttpClient, private router: Router) {
    const stored = localStorage.getItem('bunker_user');
    if (stored) {
      try { this._user.set(JSON.parse(stored)); } catch {}
    }
  }

  login(credentials: LoginRequest) {
    return this.http.post<LoginResponse>(`${API_BASE}/auth/login`, credentials, { withCredentials: true });
  }

  verifySession(): Observable<any> {
    return this.http.get(`${API_BASE}/auth/me`, { withCredentials: true });
  }

  setUser(user: UserInfo) {
    this._user.set(user);
    localStorage.setItem('bunker_user', JSON.stringify(user));
  }

  logout() {
    this.http.post(`${API_BASE}/auth/logout`, {}, { withCredentials: true }).subscribe();
    this._user.set(null);
    localStorage.removeItem('bunker_user');
    this.router.navigate(['/login']);
  }
}
