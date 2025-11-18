import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html'
})
export class CreateOrderComponent implements OnInit {
  clients: any[] = [];
  products: any[] = [];

  selectedClientId: string = '';
  // usamos um map para as quantidades por produto id
  quantities: { [productId: string]: number } = {};

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadClients();
    this.loadProducts();
  }

  loadClients() {
    this.api.getClients().subscribe(c => this.clients = c || [], err => {
      console.error('Erro ao carregar clientes', err);
    });
  }

  loadProducts() {
    this.api.getProducts().subscribe(p => {
      this.products = p || [];
      // inicializa quantidades com 1 por padrão (ou 0 se preferir)
      this.products.forEach(prod => {
        if (this.quantities[prod.id] == null) this.quantities[prod.id] = 1;
      });
    }, err => {
      console.error('Erro ao carregar produtos', err);
    });
  }

  // helper para garantir número inteiro válido >= 0
  setQuantity(productId: string, value: any) {
    // value pode vir como string do input, forçamos número inteiro
    const n = Number(value);
    if (!Number.isFinite(n) || n < 0) {
      this.quantities[productId] = 0;
    } else {
      // usar Math.trunc para evitar floats
      this.quantities[productId] = Math.trunc(n);
    }
  }

  createOrder() {
    if (!this.selectedClientId) {
      alert('Escolha um cliente');
      return;
    }

    // monta lista de produtos com quantidade >= 1
    const produtosPayload = this.products
      .map(p => {
        const q = this.quantities[p.id] ?? 0;
        return {
          id: p.id,
          quantidade: q
        };
      })
      .filter(x => x.quantidade >= 1);

    if (produtosPayload.length === 0) {
      alert('Escolha ao menos 1 produto');
      return;
    }

    const payload = {
      clienteId: this.selectedClientId,
      produtos: produtosPayload
    };

    // chamada para a API
    this.api.createOrder(payload).subscribe({
      next: (res) => {
        alert('Pedido criado');
        // limpa / recarrega
        this.selectedClientId = '';
        this.products.forEach(prod => this.quantities[prod.id] = 1);
      },
      error: (err) => {
        console.error('Erro ao criar pedido', err);
        alert('Erro ao criar pedido: ' + (err?.error?.error || err?.message || ''));
      }
    });
  }
}
