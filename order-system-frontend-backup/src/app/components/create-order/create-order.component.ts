import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html'
})
export class CreateOrderComponent implements OnInit {
  clients: any[] = [];
  products: any[] = [];
  model = {
    clienteId: '',
    produtos: [] as Array<{ id: string, quantidade: number }>
  };

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.api.getClients().subscribe(c => this.clients = c || []);
    this.api.getProducts().subscribe(p => {
      this.products = (p || []).map(x => ({ ...x, quantidade: 1 }));
    });
  }

  addOrUpdateProduto(id: string, quantidade: number) {
    const idx = this.model.produtos.findIndex(x => x.id === id);
    if (idx >= 0) this.model.produtos[idx].quantidade = quantidade;
    else this.model.produtos.push({ id, quantidade });
  }

  submit() {
    // filtra só produtos com quantidade > 0
    this.model.produtos = this.model.produtos.filter(p => p.quantidade > 0);
    if (!this.model.clienteId) { alert('Escolha um cliente'); return; }
    if (!this.model.produtos.length) { alert('Escolha ao menos 1 produto'); return; }

    this.api.createOrder(this.model).subscribe(
      r => { alert('Pedido criado: ' + r.id); this.model.produtos = []; },
      e => alert('Erro: ' + (e?.error?.error || e?.message || e))
    );
  }
}
