import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProductDto, CreateProductDto } from '../models';

const API_BASE = 'http://localhost:5005/api';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<ProductDto[]>(`${API_BASE}/products`, { withCredentials: true }); }
  getById(id: number) { return this.http.get<ProductDto>(`${API_BASE}/products/${id}`, { withCredentials: true }); }
  create(dto: CreateProductDto) { return this.http.post<ProductDto>(`${API_BASE}/products`, dto, { withCredentials: true }); }
  update(id: number, dto: CreateProductDto) { return this.http.put<ProductDto>(`${API_BASE}/products/${id}`, dto, { withCredentials: true }); }
  delete(id: number) { return this.http.delete(`${API_BASE}/products/${id}`, { withCredentials: true }); }
}
