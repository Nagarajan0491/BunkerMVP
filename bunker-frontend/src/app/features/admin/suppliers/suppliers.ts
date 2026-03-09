import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { SupplierService } from '../../../core/services/supplier.service';
import { SupplierDto } from '../../../core/models';
import { SupplierFormDialogComponent } from './supplier-form-dialog';

@Component({
  selector: 'app-suppliers',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatSortModule, MatButtonModule, MatIconModule, MatDialogModule, MatCardModule],
  template: `
    <div class="page-container">
      <div class="page-header"><h2 class="page-title">Suppliers</h2></div>
      <mat-card class="table-card">
        <table mat-table [dataSource]="dataSource" matSort class="full-width">
          <ng-container matColumnDef="name"><th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th><td mat-cell *matCellDef="let s">{{ s.name }}</td></ng-container>
          <ng-container matColumnDef="contactEmail"><th mat-header-cell *matHeaderCellDef>Email</th><td mat-cell *matCellDef="let s">{{ s.contactEmail }}</td></ng-container>
          <ng-container matColumnDef="contactPhone"><th mat-header-cell *matHeaderCellDef>Phone</th><td mat-cell *matCellDef="let s">{{ s.contactPhone }}</td></ng-container>
          <ng-container matColumnDef="country"><th mat-header-cell *matHeaderCellDef mat-sort-header>Country</th><td mat-cell *matCellDef="let s">{{ s.country }}</td></ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let s">
              <button mat-icon-button color="primary" (click)="openEdit(s)"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="warn" (click)="delete(s)"><mat-icon>delete</mat-icon></button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columns; sticky: true"></tr>
          <tr mat-row *matRowDef="let s; columns: columns;"></tr>
        </table>
      </mat-card>
      <div class="fab-container">
        <button mat-fab extended color="primary" (click)="openAdd()"><mat-icon>add</mat-icon> Add Supplier</button>
      </div>
    </div>
  `,
  styles: [`.page-header { margin-bottom: 24px; } .page-title { font-size: 20px; font-weight: 600; color: #1e293b; margin: 0; } .table-card { border-radius: 12px; overflow: hidden; box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important; } .full-width { width: 100%; }`]
})
export class SuppliersComponent implements OnInit {
  columns = ['name', 'contactEmail', 'contactPhone', 'country', 'actions'];
  dataSource = new MatTableDataSource<SupplierDto>();
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service: SupplierService, private dialog: MatDialog) {}

  ngOnInit() { this.load(); }
  ngAfterViewInit() { this.dataSource.sort = this.sort; }

  load() { this.service.getAll().subscribe(d => this.dataSource.data = d); }

  openAdd() { this.dialog.open(SupplierFormDialogComponent, { width: '480px' }).afterClosed().subscribe(r => { if (r) this.load(); }); }
  openEdit(s: SupplierDto) { this.dialog.open(SupplierFormDialogComponent, { width: '480px', data: s }).afterClosed().subscribe(r => { if (r) this.load(); }); }
  delete(s: SupplierDto) { if (confirm(`Delete "${s.name}"?`)) this.service.delete(s.id).subscribe(() => this.load()); }
}
