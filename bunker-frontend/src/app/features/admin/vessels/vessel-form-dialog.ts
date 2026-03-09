import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { VesselService } from '../../../core/services/vessel.service';
import { VesselDto } from '../../../core/models';

@Component({
  selector: 'app-vessel-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Add' }} Vessel</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="dialog-form">
        <mat-form-field appearance="outline" class="full-width"><mat-label>Name</mat-label><input matInput formControlName="name" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>IMO Number</mat-label><input matInput formControlName="imoNumber" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>Vessel Type</mat-label><input matInput formControlName="vesselType" /></mat-form-field>
        <mat-form-field appearance="outline" class="full-width"><mat-label>Flag (Country)</mat-label><input matInput formControlName="flag" /></mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" (click)="submit()" [disabled]="form.invalid">{{ data ? 'Update' : 'Create' }}</button>
    </mat-dialog-actions>
  `,
  styles: [`.dialog-form { display: flex; flex-direction: column; gap: 8px; padding-top: 8px; min-width: 400px; } .full-width { width: 100%; }`]
})
export class VesselFormDialogComponent {
  private fb = inject(FormBuilder);
  private service = inject(VesselService);
  private dialogRef = inject(MatDialogRef<VesselFormDialogComponent>);
  data: VesselDto | null = inject(MAT_DIALOG_DATA, { optional: true });

  form = this.fb.group({
    name: [this.data?.name || '', Validators.required],
    imoNumber: [this.data?.imoNumber || '', Validators.required],
    vesselType: [this.data?.vesselType || '', Validators.required],
    flag: [this.data?.flag || '', Validators.required]
  });

  submit() {
    if (this.form.invalid) return;
    const v = this.form.value as any;
    const obs = this.data ? this.service.update(this.data.id, v) : this.service.create(v);
    obs.subscribe(() => this.dialogRef.close(true));
  }
}
