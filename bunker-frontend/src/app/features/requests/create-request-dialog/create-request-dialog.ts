import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { VesselService } from '../../../core/services/vessel.service';
import { ProductService } from '../../../core/services/product.service';
import { LocationService } from '../../../core/services/location.service';
import { BunkerRequestService } from '../../../core/services/bunker-request.service';
import { VesselDto, ProductDto, LocationDto, BunkerRequestDto } from '../../../core/models';

@Component({
  selector: 'app-create-request-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatDatepickerModule],
  template: `
    <h2 mat-dialog-title>{{ editData ? 'Edit Bunker Request' : 'New Bunker Request' }}</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="dialog-form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Vessel</mat-label>
          <mat-select formControlName="vesselId">
            <mat-option *ngFor="let v of vessels" [value]="v.id">{{ v.name }}</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Product</mat-label>
          <mat-select formControlName="productId">
            <mat-option *ngFor="let p of products" [value]="p.id">{{ p.name }} ({{ p.productCode }})</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Location</mat-label>
          <mat-select formControlName="locationId">
            <mat-option *ngFor="let l of locations" [value]="l.id">{{ l.name }}</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Quantity (MT)</mat-label>
          <input matInput type="number" formControlName="quantity" />
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>ETA</mat-label>
          <input matInput [matDatepicker]="picker" formControlName="eta" />
          <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
          <mat-datepicker #picker></mat-datepicker>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" (click)="submit()" [disabled]="form.invalid">{{ editData ? 'Save' : 'Create' }}</button>
    </mat-dialog-actions>
  `,
  styles: [`.dialog-form { display: flex; flex-direction: column; gap: 8px; padding-top: 8px; min-width: 440px; } .full-width { width: 100%; }`]
})
export class CreateRequestDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private vesselService = inject(VesselService);
  private productService = inject(ProductService);
  private locationService = inject(LocationService);
  private requestService = inject(BunkerRequestService);
  private dialogRef = inject(MatDialogRef<CreateRequestDialogComponent>);
  private cdr = inject(ChangeDetectorRef);
  editData: BunkerRequestDto | null = inject(MAT_DIALOG_DATA, { optional: true });

  vessels: VesselDto[] = [];
  products: ProductDto[] = [];
  locations: LocationDto[] = [];

  form = this.fb.group({
    vesselId: [null as number | null, Validators.required],
    productId: [null as number | null, Validators.required],
    locationId: [null as number | null, Validators.required],
    quantity: [null as number | null, [Validators.required, Validators.min(1)]],
    eta: [null as Date | null, Validators.required]
  });

  ngOnInit() {
    this.vesselService.getAll().subscribe(v => { this.vessels = v; this.cdr.detectChanges(); });
    this.productService.getAll().subscribe(p => { this.products = p; this.cdr.detectChanges(); });
    this.locationService.getAll().subscribe(l => { this.locations = l; this.cdr.detectChanges(); });

    if (this.editData) {
      this.form.patchValue({
        vesselId: this.editData.vesselId,
        productId: this.editData.productId,
        locationId: this.editData.locationId,
        quantity: this.editData.quantity,
        eta: new Date(this.editData.eta)
      });
    }
  }

  submit() {
    if (this.form.invalid) return;
    const v = this.form.value;
    const payload = {
      vesselId: v.vesselId!,
      productId: v.productId!,
      locationId: v.locationId!,
      quantity: v.quantity!,
      eta: (v.eta as Date).toISOString()
    };

    if (this.editData) {
      this.requestService.update(this.editData.id, { ...payload, status: this.editData.status })
        .subscribe(() => this.dialogRef.close(true));
    } else {
      this.requestService.create(payload).subscribe(() => this.dialogRef.close(true));
    }
  }
}
