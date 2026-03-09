import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SupplierDto, CreateSupplierDto } from '../models';

const API_BASE = 'http://localhost:5005/api';

@Injectable({ providedIn: 'root' })
export class SupplierService {
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<SupplierDto[]>(`${API_BASE}/suppliers`, { withCredentials: true }); }
  getById(id: number) { return this.http.get<SupplierDto>(`${API_BASE}/suppliers/${id}`, { withCredentials: true }); }
  create(dto: CreateSupplierDto) { return this.http.post<SupplierDto>(`${API_BASE}/suppliers`, dto, { withCredentials: true }); }
  update(id: number, dto: CreateSupplierDto) { return this.http.put<SupplierDto>(`${API_BASE}/suppliers/${id}`, dto, { withCredentials: true }); }
  delete(id: number) { return this.http.delete(`${API_BASE}/suppliers/${id}`, { withCredentials: true }); }
}
