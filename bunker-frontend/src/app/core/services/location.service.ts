import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocationDto, CreateLocationDto } from '../models';

const API_BASE = 'http://localhost:5005/api';

@Injectable({ providedIn: 'root' })
export class LocationService {
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<LocationDto[]>(`${API_BASE}/locations`, { withCredentials: true }); }
  getById(id: number) { return this.http.get<LocationDto>(`${API_BASE}/locations/${id}`, { withCredentials: true }); }
  create(dto: CreateLocationDto) { return this.http.post<LocationDto>(`${API_BASE}/locations`, dto, { withCredentials: true }); }
  update(id: number, dto: CreateLocationDto) { return this.http.put<LocationDto>(`${API_BASE}/locations/${id}`, dto, { withCredentials: true }); }
  delete(id: number) { return this.http.delete(`${API_BASE}/locations/${id}`, { withCredentials: true }); }
}
