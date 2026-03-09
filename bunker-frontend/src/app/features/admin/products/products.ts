import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { ProductService } from '../../../core/services/product.service';
import { ProductDto } from '../../../core/models';
import { ProductFormDialogComponent } from './product-form-dialog';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatSortModule, MatButtonModule, MatIconModule, MatDialogModule, MatCardModule],
  template: `
    <div class="page-container">
      <div class="page-header"><h2 class="page-title">Products</h2></div>
      <mat-card class="table-card">
        <table mat-table [dataSource]="dataSource" matSort class="full-width">
          <ng-container matColumnDef="name"><th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th><td mat-cell *matCellDef="let p">{{ p.name }}</td></ng-container>
          <ng-container matColumnDef="productCode"><th mat-header-cell *matHeaderCellDef mat-sort-header>Code</th><td mat-cell *matCellDef="let p">{{ p.productCode }}</td></ng-container>
          <ng-container matColumnDef="description"><th mat-header-cell *matHeaderCellDef>Description</th><td mat-cell *matCellDef="let p">{{ p.description }}</td></ng-container>
          <ng-container matColumnDef="unit"><th mat-header-cell *matHeaderCellDef>Unit</th><td mat-cell *matCellDef="let p">{{ p.unit }}</td></ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let p">
              <button mat-icon-button color="primary" (click)="openEdit(p)"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="warn" (click)="delete(p)"><mat-icon>delete</mat-icon></button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columns; sticky: true"></tr>
          <tr mat-row *matRowDef="let p; columns: columns;"></tr>
        </table>
      </mat-card>
      <div class="fab-container">
        <button mat-fab extended color="primary" (click)="openAdd()"><mat-icon>add</mat-icon> Add Product</button>
      </div>
    </div>
  `,
  styles: [`.page-header { margin-bottom: 24px; } .page-title { font-size: 20px; font-weight: 600; color: #1e293b; margin: 0; } .table-card { border-radius: 12px; overflow: hidden; box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important; } .full-width { width: 100%; }`]
})
export class ProductsComponent implements OnInit {
  columns = ['name', 'productCode', 'description', 'unit', 'actions'];
  dataSource = new MatTableDataSource<ProductDto>();
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service: ProductService, private dialog: MatDialog) {}

  ngOnInit() { this.load(); }
  ngAfterViewInit() { this.dataSource.sort = this.sort; }

  load() { this.service.getAll().subscribe(d => this.dataSource.data = d); }

  openAdd() { this.dialog.open(ProductFormDialogComponent, { width: '480px' }).afterClosed().subscribe(r => { if (r) this.load(); }); }
  openEdit(p: ProductDto) { this.dialog.open(ProductFormDialogComponent, { width: '480px', data: p }).afterClosed().subscribe(r => { if (r) this.load(); }); }
  delete(p: ProductDto) { if (confirm(`Delete "${p.name}"?`)) this.service.delete(p.id).subscribe(() => this.load()); }
}
