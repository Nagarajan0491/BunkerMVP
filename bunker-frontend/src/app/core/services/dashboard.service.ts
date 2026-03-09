import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DashboardStatsDto } from '../models';

const API_BASE = 'http://localhost:5005/api';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private http: HttpClient) {}
  getStats() { return this.http.get<DashboardStatsDto>(`${API_BASE}/dashboard/stats`, { withCredentials: true }); }
}
