import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { SupplierService } from '../../../core/services/supplier.service';
import { SupplierDto } from '../../../core/models';

@Component({
  selector: 'app-supplier-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Add' }} Supplier</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="dialog-form">
        <mat-form-field appearance="outline" class="full-width"><mat-label>Name</mat-label><input matInput formControlName="name" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>Contact Email</mat-label><input matInput formControlName="contactEmail" type="email" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>Contact Phone</mat-label><input matInput formControlName="contactPhone" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>Country</mat-label><input matInput formControlName="country" /></mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" (click)="submit()" [disabled]="form.invalid">{{ data ? 'Update' : 'Create' }}</button>
    </mat-dialog-actions>
  `,
  styles: [`.dialog-form { display: flex; flex-direction: column; gap: 8px; padding-top: 8px; min-width: 400px; } .full-width { width: 100%; }`]
})
export class SupplierFormDialogComponent {
  private fb = inject(FormBuilder);
  private service = inject(SupplierService);
  private dialogRef = inject(MatDialogRef<SupplierFormDialogComponent>);
  data: SupplierDto | null = inject(MAT_DIALOG_DATA, { optional: true });

  form = this.fb.group({
    name: [this.data?.name || '', Validators.required],
    contactEmail: [this.data?.contactEmail || '', [Validators.required, Validators.email]],
    contactPhone: [this.data?.contactPhone || ''],
    country: [this.data?.country || '', Validators.required]
  });

  submit() {
    if (this.form.invalid) return;
    const v = this.form.value as any;
    const obs = this.data ? this.service.update(this.data.id, v) : this.service.create(v);
    obs.subscribe(() => this.dialogRef.close(true));
  }
}
