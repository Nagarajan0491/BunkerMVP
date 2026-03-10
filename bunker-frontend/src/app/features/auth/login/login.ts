import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { switchMap, delay, of } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  template: `
    <div class="login-container">
      <mat-card class="login-card">
        <mat-card-header>
          <div class="login-brand">
            <mat-icon>anchor</mat-icon>
            <h1>BunkerMVP</h1>
          </div>
          <p class="subtitle">Bunker Fuel Procurement Platform</p>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Username</mat-label>
              <input matInput formControlName="username" autocomplete="username" />
            </mat-form-field>
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Password</mat-label>
              <input matInput [type]="hidePassword ? 'password' : 'text'" formControlName="password" autocomplete="current-password" />
              <button mat-icon-button matSuffix type="button" (click)="hidePassword = !hidePassword">
                <mat-icon>{{ hidePassword ? 'visibility_off' : 'visibility' }}</mat-icon>
              </button>
            </mat-form-field>
            <div class="error-msg" *ngIf="error">{{ error }}</div>
            <button mat-flat-button color="primary" type="submit" class="full-width login-btn" [disabled]="loading">
              <mat-spinner *ngIf="loading" diameter="20" style="display:inline-block;margin-right:8px;"></mat-spinner>
              {{ loading ? 'Signing in...' : 'Sign In' }}
            </button>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .login-container {
      min-height: 100vh; display: flex; align-items: center; justify-content: center;
      background: #F8FAFC;
    }
    .login-card { width: 400px; padding: 24px; border-radius: 16px; }
    .login-brand { display: flex; align-items: center; gap: 8px; color: #1e293b; margin-bottom: 4px; }
    .login-brand mat-icon { color: #2563EB; font-size: 32px; width: 32px; height: 32px; }
    .login-brand h1 { margin: 0; font-size: 24px; font-weight: 700; }
    .subtitle { color: #64748B; margin: 0 0 24px; font-size: 14px; }
    .full-width { width: 100%; }
    .login-btn { height: 44px; margin-top: 8px; }
    .error-msg { color: #DC2626; font-size: 13px; margin-bottom: 8px; padding: 8px 12px; background: #FEF2F2; border-radius: 6px; }
  `]
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });
  loading = false;
  error = '';
  hidePassword = true;

  onSubmit() {
    if (this.form.invalid) return;
    this.loading = true;
    this.error = '';
    const { username, password } = this.form.value;
    this.auth.login({ username: username!, password: password! }).pipe(
      switchMap(res => {
        if (res.success && res.user) {
          this.auth.setUser(res.user);
          // Verify session is established before navigation with longer delay
          return this.auth.verifySession().pipe(delay(200));
        }
        throw new Error(res.message);
      })
    ).subscribe({
      next: () => {
        // Force a slight delay before navigation to ensure session cookie is set
        setTimeout(() => {
          this.router.navigate(['/dashboard']);
          this.loading = false;
        }, 100);
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Login failed. Please try again.';
        this.loading = false;
      }
    });
  }
}
