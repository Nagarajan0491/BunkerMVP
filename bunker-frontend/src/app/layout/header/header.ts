import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule],
  template: `
    <mat-toolbar class="header-toolbar">
      <span class="spacer"></span>
      <span class="user-info">{{ auth.user()?.fullName }}</span>
      <button mat-icon-button (click)="auth.logout()" title="Logout">
        <mat-icon>logout</mat-icon>
      </button>
    </mat-toolbar>
  `,
  styles: [`
    .header-toolbar {
      background: white;
      border-bottom: 1px solid #E5E7EB;
      box-shadow: 0 1px 4px rgba(0,0,0,0.04);
      height: 56px;
    }
    .spacer { flex: 1; }
    .user-info { font-size: 14px; color: #64748B; margin-right: 8px; }
  `]
})
export class HeaderComponent {
  constructor(public auth: AuthService) {}
}
