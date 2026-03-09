import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BunkerRequestService } from '../../../core/services/bunker-request.service';
import { BunkerRequestDto, SupplierQuoteDto } from '../../../core/models';
import { AddQuoteDialogComponent } from '../add-quote-dialog/add-quote-dialog';
import { CreateRequestDialogComponent } from '../create-request-dialog/create-request-dialog';

@Component({
  selector: 'app-request-detail',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatTableModule, MatButtonModule, MatIconModule, MatDialogModule, MatProgressSpinnerModule],
  template: `
    <div class="page-container">
      <div class="back-row">
        <button mat-button (click)="router.navigate(['/requests'])">
          <mat-icon>arrow_back</mat-icon> Back to Requests
        </button>
      </div>

      <div *ngIf="loading" class="loading-state">
        <mat-spinner diameter="40"></mat-spinner>
        <span>Loading request...</span>
      </div>

      <div *ngIf="!loading && !request" class="error-state">
        <mat-icon>error_outline</mat-icon>
        <span>Request not found.</span>
      </div>

      <ng-container *ngIf="request">
      <mat-card class="info-card">
        <mat-card-header>
          <mat-card-title>Request #{{ request.id }}</mat-card-title>
          <mat-card-subtitle>
            <span class="status-chip {{ request.status }}">{{ request.status }}</span>
          </mat-card-subtitle>
          <span class="header-spacer"></span>
          <button mat-stroked-button color="primary" *ngIf="request.status !== 'Ordered'" (click)="openEditDialog()">
            <mat-icon>edit</mat-icon> Edit
          </button>
        </mat-card-header>
        <mat-card-content>
          <div class="info-grid">
            <div class="info-item"><label>Vessel</label><span>{{ request.vesselName }}</span></div>
            <div class="info-item"><label>Product</label><span>{{ request.productName }}</span></div>
            <div class="info-item"><label>Location</label><span>{{ request.locationName }}</span></div>
            <div class="info-item"><label>Quantity</label><span>{{ request.quantity | number }} {{ request.productUnit }}</span></div>
            <div class="info-item"><label>ETA</label><span>{{ request.eta | date:'mediumDate' }}</span></div>
            <div class="info-item"><label>Created</label><span>{{ request.createdAt | date:'medium' }}</span></div>
          </div>
        </mat-card-content>
      </mat-card>

      <div class="section-header">
        <h3>Quotes ({{ quotes.length }})</h3>
        <button mat-stroked-button color="primary" (click)="openAddQuote()" [disabled]="request.status === 'Ordered'">
          <mat-icon>add</mat-icon> Add Quote
        </button>
      </div>

      <mat-card class="table-card" *ngIf="quotes.length > 0">
        <table mat-table [dataSource]="quotes" class="full-width">
          <ng-container matColumnDef="supplier"><th mat-header-cell *matHeaderCellDef>Supplier</th><td mat-cell *matCellDef="let q">{{ q.supplierName }}</td></ng-container>
          <ng-container matColumnDef="price"><th mat-header-cell *matHeaderCellDef>Price/MT</th><td mat-cell *matCellDef="let q">{{ q.price | number:'1.2-2' }} {{ q.currency }}</td></ng-container>
          <ng-container matColumnDef="validUntil"><th mat-header-cell *matHeaderCellDef>Valid Until</th><td mat-cell *matCellDef="let q">{{ q.validUntil | date:'mediumDate' }}</td></ng-container>
          <ng-container matColumnDef="notes"><th mat-header-cell *matHeaderCellDef>Notes</th><td mat-cell *matCellDef="let q">{{ q.notes }}</td></ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let q">
              <button mat-flat-button color="primary" (click)="createOrder(q)" [disabled]="request.status === 'Ordered'" style="font-size:12px;height:32px;line-height:32px;">
                Create Order
              </button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="quoteColumns"></tr>
          <tr mat-row *matRowDef="let q; columns: quoteColumns;"></tr>
        </table>
      </mat-card>
      <p *ngIf="quotes.length === 0" class="no-data">No quotes yet. Add the first quote.</p>
      </ng-container>
    </div>
  `,
  styles: [`
    .back-row { margin-bottom: 16px; }
    .loading-state { display: flex; align-items: center; gap: 16px; padding: 48px; color: #64748B; }
    .error-state { display: flex; align-items: center; gap: 8px; padding: 48px; color: #EF4444; }
    .info-card { border-radius: 12px; margin-bottom: 24px; box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important; }
    .header-spacer { flex: 1 1 auto; }
    .info-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; padding-top: 16px; }
    .info-item label { display: block; font-size: 11px; text-transform: uppercase; color: #94A3B8; font-weight: 600; margin-bottom: 4px; }
    .info-item span { font-size: 14px; color: #1e293b; font-weight: 500; }
    .section-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 12px; }
    .section-header h3 { margin: 0; font-size: 16px; font-weight: 600; color: #1e293b; }
    .table-card { border-radius: 12px; overflow: hidden; box-shadow: 0 1px 8px rgba(0,0,0,0.08) !important; }
    .full-width { width: 100%; }
    .no-data { color: #94A3B8; text-align: center; padding: 24px; }
  `]
})
export class RequestDetailComponent implements OnInit {
  request: BunkerRequestDto | null = null;
  quotes: SupplierQuoteDto[] = [];
  loading = true;
  quoteColumns = ['supplier', 'price', 'validUntil', 'notes', 'actions'];

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private service: BunkerRequestService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    const id = +this.route.snapshot.params['id'];
    this.loadRequest(id);
    this.loadQuotes(id);
  }

  loadRequest(id: number) {
    this.loading = true;
    this.service.getById(id).subscribe({
      next: r => { this.request = r; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  loadQuotes(id: number) {
    this.service.getQuotes(id).subscribe({
      next: q => this.quotes = q ?? [],
      error: () => { this.quotes = []; }
    });
  }

  openEditDialog() {
    const ref = this.dialog.open(CreateRequestDialogComponent, {
      width: '520px',
      data: this.request
    });
    ref.afterClosed().subscribe(result => {
      if (result) this.loadRequest(this.request!.id);
    });
  }

  openAddQuote() {
    const ref = this.dialog.open(AddQuoteDialogComponent, {
      width: '480px',
      data: { requestId: this.request!.id }
    });
    ref.afterClosed().subscribe(result => {
      if (result) {
        this.loadRequest(this.request!.id);
        this.loadQuotes(this.request!.id);
      }
    });
  }

  createOrder(quote: SupplierQuoteDto) {
    this.service.createOrder(this.request!.id, { supplierQuoteId: quote.id, notes: '' }).subscribe(() => {
      this.loadRequest(this.request!.id);
    });
  }
}
