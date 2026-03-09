import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { LocationService } from '../../../core/services/location.service';
import { LocationDto } from '../../../core/models';
import { LocationFormDialogComponent } from './location-form-dialog';

@Component({
  selector: 'app-locations',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatSortModule, MatButtonModule, MatIconModule, MatDialogModule, MatCardModule],
  template: `
    <div class="page-container">
      <div class="page-header"><h2 class="page-title">Locations</h2></div>
      <mat-card class="table-card">
        <table mat-table [dataSource]="dataSource" matSort class="full-width">
          <ng-container matColumnDef="name"><th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th><td mat-cell *matCellDef="let l">{{ l.name }}</td></ng-container>
          <ng-container matColumnDef="port"><th mat-header-cell *matHeaderCellDef mat-sort-header>Port</th><td mat-cell *matCellDef="let l">{{ l.port }}</td></ng-container>
          <ng-container matColumnDef="country"><th mat-header-cell *matHeaderCellDef mat-sort-header>Country</th><td mat-cell *matCellDef="let l">{{ l.country }}</td></ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let l">
              <button mat-icon-button color="primary" (click)="openEdit(l)"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="warn" (click)="delete(l)"><mat-icon>delete</mat-icon></button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columns; sticky: true"></tr>
          <tr mat-row *matRowDef="let l; columns: columns;"></tr>
        </table>
      </mat-card>
      <div class="fab-container">
        <button mat-fab extended color="primary" (click)="openAdd()"><mat-icon>add</mat-icon> Add Location</button>
      </div>
    </div>
  `,
  styles: [`.page-header { margin-bottom: 24px; } .page-title { font-size: 20px; font-weight: 600; color: #1e293b; margin: 0; } .table-card { border-radius: 12px; overflow: hidden; box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important; } .full-width { width: 100%; }`]
})
export class LocationsComponent implements OnInit {
  columns = ['name', 'port', 'country', 'actions'];
  dataSource = new MatTableDataSource<LocationDto>();
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service: LocationService, private dialog: MatDialog) {}

  ngOnInit() { this.load(); }
  ngAfterViewInit() { this.dataSource.sort = this.sort; }

  load() { this.service.getAll().subscribe(d => this.dataSource.data = d); }

  openAdd() { this.dialog.open(LocationFormDialogComponent, { width: '480px' }).afterClosed().subscribe(r => { if (r) this.load(); }); }
  openEdit(l: LocationDto) { this.dialog.open(LocationFormDialogComponent, { width: '480px', data: l }).afterClosed().subscribe(r => { if (r) this.load(); }); }
  delete(l: LocationDto) { if (confirm(`Delete "${l.name}"?`)) this.service.delete(l.id).subscribe(() => this.load()); }
}
