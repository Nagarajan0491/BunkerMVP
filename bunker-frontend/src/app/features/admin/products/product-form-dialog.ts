import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ProductService } from '../../../core/services/product.service';
import { ProductDto } from '../../../core/models';

@Component({
  selector: 'app-product-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Add' }} Product</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="dialog-form">
        <mat-form-field appearance="outline" class="full-width"><mat-label>Name</mat-label><input matInput formControlName="name" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>Product Code</mat-label><input matInput formControlName="productCode" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>Description</mat-label><input matInput formControlName="description" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Unit</mat-label>
          <mat-select formControlName="unit">
            <mat-option value="MT">MT</mat-option>
            <mat-option value="L">L</mat-option>
          </mat-select>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" (click)="submit()" [disabled]="form.invalid">{{ data ? 'Update' : 'Create' }}</button>
    </mat-dialog-actions>
  `,
  styles: [`.dialog-form { display: flex; flex-direction: column; gap: 8px; padding-top: 8px; min-width: 400px; } .full-width { width: 100%; }`]
})
export class ProductFormDialogComponent {
  private fb = inject(FormBuilder);
  private service = inject(ProductService);
  private dialogRef = inject(MatDialogRef<ProductFormDialogComponent>);
  data: ProductDto | null = inject(MAT_DIALOG_DATA, { optional: true });

  form = this.fb.group({
    name: [this.data?.name || '', Validators.required],
    productCode: [this.data?.productCode || '', Validators.required],
    description: [this.data?.description || ''],
    unit: [this.data?.unit || 'MT', Validators.required]
  });

  submit() {
    if (this.form.invalid) return;
    const v = this.form.value as any;
    const obs = this.data ? this.service.update(this.data.id, v) : this.service.create(v);
    obs.subscribe(() => this.dialogRef.close(true));
  }
}
