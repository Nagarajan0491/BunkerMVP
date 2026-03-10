import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { finalize } from 'rxjs';
import { DashboardService } from '../../core/services/dashboard.service';
import { DashboardStatsDto } from '../../core/models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatProgressSpinnerModule],
  template: `
    <div class="page-container">
      <h2 class="page-title">Dashboard</h2>

      <div *ngIf="loading" class="loading-state">
        <mat-spinner diameter="40"></mat-spinner>
      </div>

      <div *ngIf="!loading && error" class="error-state">
        <mat-icon>error_outline</mat-icon>
        <span>Failed to load dashboard stats. Please refresh the page.</span>
      </div>

      <div class="stats-grid" *ngIf="!loading && stats">
        <mat-card class="stat-card">
          <div class="stat-icon blue"><mat-icon>assignment</mat-icon></div>
          <div class="stat-info">
            <div class="stat-value">{{ stats!.totalRequests }}</div>
            <div class="stat-label">Total Requests</div>
          </div>
        </mat-card>
        <mat-card class="stat-card">
          <div class="stat-icon amber"><mat-icon>pending_actions</mat-icon></div>
          <div class="stat-info">
            <div class="stat-value">{{ stats!.openRequests }}</div>
            <div class="stat-label">Open Requests</div>
          </div>
        </mat-card>
        <mat-card class="stat-card">
          <div class="stat-icon purple"><mat-icon>request_quote</mat-icon></div>
          <div class="stat-info">
            <div class="stat-value">{{ stats!.pendingQuotes }}</div>
            <div class="stat-label">Pending Quotes</div>
          </div>
        </mat-card>
        <mat-card class="stat-card">
          <div class="stat-icon green"><mat-icon>check_circle</mat-icon></div>
          <div class="stat-info">
            <div class="stat-value">{{ stats!.totalOrders }}</div>
            <div class="stat-label">Total Orders</div>
          </div>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .page-title { font-size: 20px; font-weight: 600; color: #1e293b; margin: 0 0 24px; }
    .loading-state { display: flex; justify-content: center; padding: 48px; }
    .error-state { display: flex; align-items: center; gap: 8px; padding: 24px; color: #EF4444; }
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(220px, 1fr)); gap: 16px; }
    .stat-card {
      display: flex; align-items: center; gap: 16px;
      padding: 20px; border-radius: 12px;
      box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important;
    }
    .stat-icon {
      width: 48px; height: 48px; border-radius: 12px;
      display: flex; align-items: center; justify-content: center;
      flex-shrink: 0;
    }
    .stat-icon.blue { background: #EFF6FF; color: #2563EB; }
    .stat-icon.amber { background: #FFFBEB; color: #D97706; }
    .stat-icon.purple { background: #F5F3FF; color: #7C3AED; }
    .stat-icon.green { background: #F0FDF4; color: #16A34A; }
    .stat-value { font-size: 28px; font-weight: 700; color: #1e293b; line-height: 1; }
    .stat-label { font-size: 13px; color: #64748B; margin-top: 4px; }
  `]
})
export class DashboardComponent implements OnInit {
  stats: DashboardStatsDto | null = null;
  loading = true;
  error = false;

  constructor(
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.dashboardService.getStats().pipe(
      finalize(() => {
        this.loading = false;
        this.cdr.detectChanges();
      })
    ).subscribe({
      next: s => this.stats = s,
      error: () => this.error = true
    });
  }
}
