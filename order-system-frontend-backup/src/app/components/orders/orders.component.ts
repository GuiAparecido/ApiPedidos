import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent implements OnInit {
  orders: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.api.getOrders().subscribe(
      r => this.orders = r || [],
      err => {
        console.error('Erro ao carregar pedidos', err);
        this.orders = [];
      }
    );
  }

  pay(id: string) {
    this.api.payOrder(id).subscribe(
      () => this.load(),
      () => alert('Erro ao pagar')
    );
  }

  cancel(id: string) {
    this.api.cancelOrder(id).subscribe(
      () => this.load(),
      () => alert('Erro ao cancelar')
    );
  }

  viewTotal(id: string) {
    this.api.getTotal(id).subscribe(
      t => alert('Total: ' + t),
      () => alert('Erro ao buscar total')
    );
  }

  // mapeamento de status numérico -> texto
  statusMap: any = {
    0: 'Falta pagar',
    1: 'Pago',
    2: 'Cancelado'
  };
}
