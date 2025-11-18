import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
  newProduct: any = { nome: '', preco: 0 };

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.api.getProducts().subscribe(
      p => this.products = p || [],
      err => {
        console.error('Erro ao carregar produtos', err);
        this.products = [];
      }
    );
  }

  save() {
    // validação simples
    if (!this.newProduct || !this.newProduct.nome) {
      alert('Informe o nome do produto');
      return;
    }
    if (this.newProduct.preco == null || this.newProduct.preco < 0) {
      alert('Informe um preço válido');
      return;
    }

    this.api.createProduct(this.newProduct).subscribe(
      () => {
        this.newProduct = { nome: '', preco: 0 };
        this.load();
      },
      err => {
        console.error('Erro ao criar produto', err);
        alert('Erro ao criar produto');
      }
    );
  }
}
