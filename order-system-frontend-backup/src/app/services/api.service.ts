import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const API = 'http://localhost:5000';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  // PEDIDOS
  getOrders(): Observable<any[]> { return this.http.get<any[]>(`${API}/pedidos`); }
  getOrder(id: string) { return this.http.get<any>(`${API}/pedidos/${id}`); }
  payOrder(id: string) { return this.http.post(`${API}/pedidos/${id}/pagar`, {}); }
  cancelOrder(id: string) { return this.http.post(`${API}/pedidos/${id}/cancelar`, {}); }
  getTotal(id: string) { return this.http.get<number>(`${API}/pedidos/${id}/total`); }
  createOrder(dto: any) { return this.http.post<any>(`${API}/pedidos`, dto); }

  // CLIENTES
  getClients(): Observable<any[]> { return this.http.get<any[]>(`${API}/clientes`); }
  getClient(id: string) { return this.http.get<any>(`${API}/clientes/${id}`); }
  // <<-- ADICIONE este método chamado pelos componentes
  createClient(dto: any) { return this.http.post<any>(`${API}/clientes`, dto); }
  // opcional: update/delete se precisar
  updateClient(id: string, dto: any) { return this.http.put<any>(`${API}/clientes/${id}`, dto); }
  deleteClient(id: string) { return this.http.delete(`${API}/clientes/${id}`); }

  // PRODUTOS
  getProducts(): Observable<any[]> { return this.http.get<any[]>(`${API}/produtos`); }
  getProduct(id: string) { return this.http.get<any>(`${API}/produtos/${id}`); }
  // <<-- ADICIONE este método chamado pelos componentes
  createProduct(dto: any) { return this.http.post<any>(`${API}/produtos`, dto); }
  // opcional: update/delete se precisar
  updateProduct(id: string, dto: any) { return this.http.put<any>(`${API}/produtos/${id}`, dto); }
  deleteProduct(id: string) { return this.http.delete(`${API}/produtos/${id}`); }
}