import { Component, Inject, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { SupplierService } from '../../../core/services/supplier.service';
import { BunkerRequestService } from '../../../core/services/bunker-request.service';
import { SupplierDto } from '../../../core/models';

@Component({
  selector: 'app-add-quote-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatDatepickerModule, MatNativeDateModule],
  template: `
    <h2 mat-dialog-title>Add Quote</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="dialog-form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Supplier</mat-label>
          <mat-select formControlName="supplierId">
            <mat-option *ngFor="let s of suppliers" [value]="s.id">{{ s.name }}</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Price per MT</mat-label>
          <input matInput type="number" formControlName="price" />
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Currency</mat-label>
          <mat-select formControlName="currency">
            <mat-option value="USD">USD</mat-option>
            <mat-option value="EUR">EUR</mat-option>
            <mat-option value="SGD">SGD</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Valid Until</mat-label>
          <input matInput [matDatepicker]="picker" formControlName="validUntil" />
          <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
          <mat-datepicker #picker></mat-datepicker>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Notes</mat-label>
          <textarea matInput formControlName="notes" rows="3"></textarea>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" (click)="submit()" [disabled]="form.invalid">Add Quote</button>
    </mat-dialog-actions>
  `,
  styles: [`.dialog-form { display: flex; flex-direction: column; gap: 8px; padding-top: 8px; min-width: 420px; } .full-width { width: 100%; }`]
})
export class AddQuoteDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private supplierService = inject(SupplierService);
  private requestService = inject(BunkerRequestService);
  private dialogRef = inject(MatDialogRef<AddQuoteDialogComponent>);
  private data: { requestId: number } = inject(MAT_DIALOG_DATA);

  suppliers: SupplierDto[] = [];

  form = this.fb.group({
    supplierId: [null as number | null, Validators.required],
    price: [null as number | null, [Validators.required, Validators.min(0.01)]],
    currency: ['USD', Validators.required],
    validUntil: [null as Date | null, Validators.required],
    notes: ['']
  });

  ngOnInit() {
    this.supplierService.getAll().subscribe(s => this.suppliers = s);
  }

  submit() {
    if (this.form.invalid) return;
    const v = this.form.value;
    this.requestService.addQuote(this.data.requestId, {
      supplierId: v.supplierId!,
      price: v.price!,
      currency: v.currency!,
      validUntil: (v.validUntil as Date).toISOString(),
      notes: v.notes || ''
    }).subscribe(() => this.dialogRef.close(true));
  }
}
