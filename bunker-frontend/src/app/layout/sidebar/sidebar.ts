import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, MatIconModule, CommonModule],
  template: `
    <div class="sidebar-wrapper">
      <div class="brand">
        <mat-icon>anchor</mat-icon>
        <span>BunkerMVP</span>
      </div>
      <nav class="nav-list">
        <a *ngFor="let item of navItems"
           [routerLink]="item.route"
           routerLinkActive="active"
           class="nav-item">
          <mat-icon>{{ item.icon }}</mat-icon>
          <span>{{ item.label }}</span>
        </a>
      </nav>
      <div class="nav-section-label">ADMIN</div>
      <nav class="nav-list">
        <a *ngFor="let item of adminItems"
           [routerLink]="item.route"
           routerLinkActive="active"
           class="nav-item">
          <mat-icon>{{ item.icon }}</mat-icon>
          <span>{{ item.label }}</span>
        </a>
      </nav>
    </div>
  `,
  styles: [`
    .sidebar-wrapper { height: 100%; display: flex; flex-direction: column; padding: 16px 0; }
    .brand {
      display: flex; align-items: center; gap: 8px;
      padding: 8px 20px 24px;
      color: white; font-size: 18px; font-weight: 600;
    }
    .brand mat-icon { color: #60A5FA; }
    .nav-section-label {
      padding: 16px 20px 4px;
      font-size: 10px; font-weight: 600; color: #94A3B8;
      letter-spacing: 0.1em;
    }
    .nav-list { display: flex; flex-direction: column; gap: 2px; padding: 0 8px; }
    .nav-item {
      display: flex; align-items: center; gap: 12px;
      padding: 10px 12px; border-radius: 8px;
      color: #CBD5E1; text-decoration: none;
      font-size: 14px; font-weight: 500;
      transition: all 0.15s;
    }
    .nav-item:hover { background: rgba(255,255,255,0.08); color: white; }
    .nav-item.active { background: rgba(37,99,235,0.25); color: #93C5FD; }
    .nav-item mat-icon { font-size: 20px; width: 20px; height: 20px; }
  `]
})
export class SidebarComponent {
  navItems: NavItem[] = [
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' },
    { label: 'Requests', icon: 'assignment', route: '/requests' },
  ];
  adminItems: NavItem[] = [
    { label: 'Vessels', icon: 'directions_boat', route: '/admin/vessels' },
    { label: 'Products', icon: 'local_gas_station', route: '/admin/products' },
    { label: 'Locations', icon: 'location_on', route: '/admin/locations' },
    { label: 'Suppliers', icon: 'business', route: '/admin/suppliers' },
  ];
}
