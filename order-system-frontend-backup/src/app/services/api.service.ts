import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const API = 'http://localhost:5000';

const JSON_HEADERS = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  // PEDIDOS
  getOrders(): Observable<any[]> { return this.http.get<any[]>(`${API}/pedidos`); }
  getOrder(id: string) { return this.http.get<any>(`${API}/pedidos/${id}`); }
  payOrder(id: string) { return this.http.post(`${API}/pedidos/${id}/pagar`, {}, JSON_HEADERS); }
  cancelOrder(id: string) { return this.http.post(`${API}/pedidos/${id}/cancelar`, {}, JSON_HEADERS); }
  getTotal(id: string) { return this.http.get<number>(`${API}/pedidos/${id}/total`); }
  createOrder(dto: any) { return this.http.post<any>(`${API}/pedidos`, dto, JSON_HEADERS); }

  // CLIENTES
  getClients(): Observable<any[]> { return this.http.get<any[]>(`${API}/clientes`); }
  getClient(id: string) { return this.http.get<any>(`${API}/clientes/${id}`); }
  createClient(dto: any) { return this.http.post<any>(`${API}/clientes`, dto, JSON_HEADERS); }
  updateClient(id: string, dto: any) { return this.http.put<any>(`${API}/clientes/${id}`, dto, JSON_HEADERS); }
  deleteClient(id: string) { return this.http.delete(`${API}/clientes/${id}`); }

  // PRODUTOS
  getProducts(): Observable<any[]> { return this.http.get<any[]>(`${API}/produtos`); }
  getProduct(id: string) { return this.http.get<any>(`${API}/produtos/${id}`); }
  createProduct(dto: any) { return this.http.post<any>(`${API}/produtos`, dto, JSON_HEADERS); }
  updateProduct(id: string, dto: any) { return this.http.put<any>(`${API}/produtos/${id}`, dto, JSON_HEADERS); }
  deleteProduct(id: string) { return this.http.delete(`${API}/produtos/${id}`); }
}
