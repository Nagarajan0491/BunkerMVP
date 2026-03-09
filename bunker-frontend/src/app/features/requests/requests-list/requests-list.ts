import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { BunkerRequestService } from '../../../core/services/bunker-request.service';
import { BunkerRequestDto } from '../../../core/models';
import { CreateRequestDialogComponent } from '../create-request-dialog/create-request-dialog';

@Component({
  selector: 'app-requests-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatSortModule, MatPaginatorModule, MatButtonModule, MatIconModule, MatDialogModule, MatCardModule],
  template: `
    <div class="page-container">
      <div class="page-header">
        <h2 class="page-title">Bunker Requests</h2>
        <button mat-flat-button color="primary" (click)="openCreateDialog()">
          <mat-icon>add</mat-icon> New Request
        </button>
      </div>
      <mat-card class="table-card">
        <table mat-table [dataSource]="dataSource" matSort class="full-width">
          <ng-container matColumnDef="id">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>ID</th>
            <td mat-cell *matCellDef="let r">#{{ r.id }}</td>
          </ng-container>
          <ng-container matColumnDef="vesselName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Vessel</th>
            <td mat-cell *matCellDef="let r">{{ r.vesselName }}</td>
          </ng-container>
          <ng-container matColumnDef="productName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Product</th>
            <td mat-cell *matCellDef="let r">{{ r.productName }}</td>
          </ng-container>
          <ng-container matColumnDef="locationName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Location</th>
            <td mat-cell *matCellDef="let r">{{ r.locationName }}</td>
          </ng-container>
          <ng-container matColumnDef="quantity">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Quantity</th>
            <td mat-cell *matCellDef="let r">{{ r.quantity | number }} {{ r.productUnit }}</td>
          </ng-container>
          <ng-container matColumnDef="eta">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>ETA</th>
            <td mat-cell *matCellDef="let r">{{ r.eta | date:'mediumDate' }}</td>
          </ng-container>
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
            <td mat-cell *matCellDef="let r">
              <span class="status-chip {{ r.status }}">{{ r.status }}</span>
            </td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let r">
              <button mat-icon-button color="primary" (click)="viewDetail(r, $event)">
                <mat-icon>chevron_right</mat-icon>
              </button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="displayedColumns; sticky: true"></tr>
          <tr mat-row *matRowDef="let r; columns: displayedColumns;" (click)="viewDetail(r, $event)" style="cursor:pointer"></tr>
        </table>
        <mat-paginator [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
      </mat-card>
    </div>
  `,
  styles: [`
    .page-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 24px; }
    .page-title { font-size: 20px; font-weight: 600; color: #1e293b; margin: 0; }
    .table-card { border-radius: 12px; overflow: hidden; box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important; }
    .full-width { width: 100%; }
  `]
})
export class RequestsListComponent implements OnInit {
  displayedColumns = ['id', 'vesselName', 'productName', 'locationName', 'quantity', 'eta', 'status', 'actions'];
  dataSource = new MatTableDataSource<BunkerRequestDto>();

  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private service: BunkerRequestService, private dialog: MatDialog, private router: Router) {}

  ngOnInit() { this.loadRequests(); }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  loadRequests() {
    this.service.getAll().subscribe(data => this.dataSource.data = data);
  }

  openCreateDialog() {
    const ref = this.dialog.open(CreateRequestDialogComponent, { width: '520px' });
    ref.afterClosed().subscribe(result => { if (result) this.loadRequests(); });
  }

  viewDetail(r: BunkerRequestDto, event: Event) {
    event.stopPropagation();
    this.router.navigate(['/requests', r.id]);
  }
}
