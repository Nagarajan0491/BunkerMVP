import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./features/auth/login/login').then(m => m.LoginComponent) },
  {
    path: '',
    loadComponent: () => import('./layout/shell/shell').then(m => m.ShellComponent),
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard').then(m => m.DashboardComponent) },
      { path: 'requests', loadComponent: () => import('./features/requests/requests-list/requests-list').then(m => m.RequestsListComponent) },
      { path: 'requests/:id', loadComponent: () => import('./features/requests/request-detail/request-detail').then(m => m.RequestDetailComponent) },
      { path: 'admin/vessels', loadComponent: () => import('./features/admin/vessels/vessels').then(m => m.VesselsComponent) },
      { path: 'admin/products', loadComponent: () => import('./features/admin/products/products').then(m => m.ProductsComponent) },
      { path: 'admin/locations', loadComponent: () => import('./features/admin/locations/locations').then(m => m.LocationsComponent) },
      { path: 'admin/suppliers', loadComponent: () => import('./features/admin/suppliers/suppliers').then(m => m.SuppliersComponent) },
    ]
  },
  { path: '**', redirectTo: '/dashboard' }
];
