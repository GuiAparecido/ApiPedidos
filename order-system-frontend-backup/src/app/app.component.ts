import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
  <div class="container">
    <nav class="navbar navbar-expand-lg navbar-light bg-light mb-3 p-2 rounded">
      <a class="navbar-brand" href="#">Order System</a>
      <div>
        <a class="btn btn-outline-primary btn-sm me-2" routerLink="/orders">Pedidos</a>
<a class="btn btn-outline-secondary btn-sm me-2" routerLink="/orders/create">Criar Pedido</a>
<a class="btn btn-outline-success btn-sm me-2" routerLink="/products">Produtos</a>
<a class="btn btn-outline-info btn-sm" routerLink="/clients">Clientes</a>
      </div>
    </nav>

    <router-outlet></router-outlet>
  </div>
  `
})
export class AppComponent {}
