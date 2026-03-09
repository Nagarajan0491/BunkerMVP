export interface UserInfo {
  id: number;
  username: string;
  fullName: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  message: string;
  user?: UserInfo;
}

export interface VesselDto {
  id: number;
  name: string;
  imoNumber: string;
  vesselType: string;
  flag: string;
  createdAt: string;
}

export interface CreateVesselDto {
  name: string;
  imoNumber: string;
  vesselType: string;
  flag: string;
}

export interface ProductDto {
  id: number;
  name: string;
  productCode: string;
  description: string;
  unit: string;
  createdAt: string;
}

export interface CreateProductDto {
  name: string;
  productCode: string;
  description: string;
  unit: string;
}

export interface LocationDto {
  id: number;
  name: string;
  port: string;
  country: string;
  createdAt: string;
}

export interface CreateLocationDto {
  name: string;
  port: string;
  country: string;
}

export interface SupplierDto {
  id: number;
  name: string;
  contactEmail: string;
  contactPhone: string;
  country: string;
  createdAt: string;
}

export interface CreateSupplierDto {
  name: string;
  contactEmail: string;
  contactPhone: string;
  country: string;
}

export interface BunkerRequestDto {
  id: number;
  vesselId: number;
  vesselName: string;
  productId: number;
  productName: string;
  productUnit: string;
  locationId: number;
  locationName: string;
  quantity: number;
  eta: string;
  status: string;
  quoteCount: number;
  hasOrder: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateBunkerRequestDto {
  vesselId: number;
  productId: number;
  locationId: number;
  quantity: number;
  eta: string;
}

export interface UpdateBunkerRequestDto {
  vesselId: number;
  productId: number;
  locationId: number;
  quantity: number;
  eta: string;
  status: string;
}

export interface SupplierQuoteDto {
  id: number;
  bunkerRequestId: number;
  supplierId: number;
  supplierName: string;
  price: number;
  currency: string;
  validUntil: string;
  notes: string;
  createdAt: string;
}

export interface CreateQuoteDto {
  supplierId: number;
  price: number;
  currency: string;
  validUntil: string;
  notes: string;
}

export interface BunkerOrderDto {
  id: number;
  bunkerRequestId: number;
  supplierQuoteId: number;
  orderDate: string;
  totalAmount: number;
  currency: string;
  status: string;
  notes: string;
  createdAt: string;
}

export interface CreateOrderDto {
  supplierQuoteId: number;
  notes: string;
}

export interface DashboardStatsDto {
  totalRequests: number;
  openRequests: number;
  pendingQuotes: number;
  totalOrders: number;
}
