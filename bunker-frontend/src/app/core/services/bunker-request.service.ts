import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BunkerRequestDto, CreateBunkerRequestDto, UpdateBunkerRequestDto, SupplierQuoteDto, CreateQuoteDto, BunkerOrderDto, CreateOrderDto } from '../models';

const API_BASE = 'http://localhost:5005/api';

@Injectable({ providedIn: 'root' })
export class BunkerRequestService {
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<BunkerRequestDto[]>(`${API_BASE}/requests`, { withCredentials: true }); }
  getById(id: number) { return this.http.get<BunkerRequestDto>(`${API_BASE}/requests/${id}`, { withCredentials: true }); }
  create(dto: CreateBunkerRequestDto) { return this.http.post<BunkerRequestDto>(`${API_BASE}/requests`, dto, { withCredentials: true }); }
  update(id: number, dto: UpdateBunkerRequestDto) { return this.http.put<BunkerRequestDto>(`${API_BASE}/requests/${id}`, dto, { withCredentials: true }); }
  delete(id: number) { return this.http.delete(`${API_BASE}/requests/${id}`, { withCredentials: true }); }
  getQuotes(id: number) { return this.http.get<SupplierQuoteDto[]>(`${API_BASE}/requests/${id}/quotes`, { withCredentials: true }); }
  addQuote(id: number, dto: CreateQuoteDto) { return this.http.post<SupplierQuoteDto>(`${API_BASE}/requests/${id}/quotes`, dto, { withCredentials: true }); }
  createOrder(id: number, dto: CreateOrderDto) { return this.http.post<BunkerOrderDto>(`${API_BASE}/requests/${id}/orders`, dto, { withCredentials: true }); }
}
