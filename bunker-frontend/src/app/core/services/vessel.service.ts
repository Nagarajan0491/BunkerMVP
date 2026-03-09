import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { VesselDto, CreateVesselDto } from '../models';

const API_BASE = 'http://localhost:5005/api';

@Injectable({ providedIn: 'root' })
export class VesselService {
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<VesselDto[]>(`${API_BASE}/vessels`, { withCredentials: true }); }
  getById(id: number) { return this.http.get<VesselDto>(`${API_BASE}/vessels/${id}`, { withCredentials: true }); }
  create(dto: CreateVesselDto) { return this.http.post<VesselDto>(`${API_BASE}/vessels`, dto, { withCredentials: true }); }
  update(id: number, dto: CreateVesselDto) { return this.http.put<VesselDto>(`${API_BASE}/vessels/${id}`, dto, { withCredentials: true }); }
  delete(id: number) { return this.http.delete(`${API_BASE}/vessels/${id}`, { withCredentials: true }); }
}
