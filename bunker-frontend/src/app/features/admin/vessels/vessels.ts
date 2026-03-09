import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { VesselService } from '../../../core/services/vessel.service';
import { VesselDto } from '../../../core/models';
import { VesselFormDialogComponent } from './vessel-form-dialog';

@Component({
  selector: 'app-vessels',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatSortModule, MatButtonModule, MatIconModule, MatDialogModule, MatCardModule],
  template: `
    <div class="page-container">
      <div class="page-header">
        <h2 class="page-title">Vessels</h2>
      </div>
      <mat-card class="table-card">
        <table mat-table [dataSource]="dataSource" matSort class="full-width">
          <ng-container matColumnDef="name"><th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th><td mat-cell *matCellDef="let v">{{ v.name }}</td></ng-container>
          <ng-container matColumnDef="imoNumber"><th mat-header-cell *matHeaderCellDef mat-sort-header>IMO Number</th><td mat-cell *matCellDef="let v">{{ v.imoNumber }}</td></ng-container>
          <ng-container matColumnDef="vesselType"><th mat-header-cell *matHeaderCellDef mat-sort-header>Type</th><td mat-cell *matCellDef="let v">{{ v.vesselType }}</td></ng-container>
          <ng-container matColumnDef="flag"><th mat-header-cell *matHeaderCellDef mat-sort-header>Flag</th><td mat-cell *matCellDef="let v">{{ v.flag }}</td></ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let v">
              <button mat-icon-button color="primary" (click)="openEdit(v)"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="warn" (click)="delete(v)"><mat-icon>delete</mat-icon></button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columns; sticky: true"></tr>
          <tr mat-row *matRowDef="let v; columns: columns;"></tr>
        </table>
      </mat-card>
      <div class="fab-container">
        <button mat-fab extended color="primary" (click)="openAdd()">
          <mat-icon>add</mat-icon> Add Vessel
        </button>
      </div>
    </div>
  `,
  styles: [`
    .page-header { margin-bottom: 24px; }
    .page-title { font-size: 20px; font-weight: 600; color: #1e293b; margin: 0; }
    .table-card { border-radius: 12px; overflow: hidden; box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important; }
    .full-width { width: 100%; }
  `]
})
export class VesselsComponent implements OnInit {
  columns = ['name', 'imoNumber', 'vesselType', 'flag', 'actions'];
  dataSource = new MatTableDataSource<VesselDto>();
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service: VesselService, private dialog: MatDialog) {}

  ngOnInit() { this.load(); }
  ngAfterViewInit() { this.dataSource.sort = this.sort; }

  load() { this.service.getAll().subscribe(d => this.dataSource.data = d); }

  openAdd() {
    this.dialog.open(VesselFormDialogComponent, { width: '480px' }).afterClosed().subscribe(r => { if (r) this.load(); });
  }

  openEdit(v: VesselDto) {
    this.dialog.open(VesselFormDialogComponent, { width: '480px', data: v }).afterClosed().subscribe(r => { if (r) this.load(); });
  }

  delete(v: VesselDto) {
    if (confirm(`Delete vessel "${v.name}"?`)) {
      this.service.delete(v.id).subscribe(() => this.load());
    }
  }
}
