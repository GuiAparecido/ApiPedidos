import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html'
})
export class ClientsComponent implements OnInit {
  clients: any[] = [];
  newClient = { nome: '', cpf: '' };

  constructor(private api: ApiService) {}
  ngOnInit() { this.load(); }
  load() { this.api.getClients().subscribe(r => this.clients = r || []); }

  save() {
    if (!this.newClient.nome || !this.newClient.cpf) return alert('Preencha Nome/CPF');
    this.api.createClient(this.newClient).subscribe(() => {
      this.newClient = { nome: '', cpf: '' };
      this.load();
    }, e => alert('Erro ao criar cliente'));
  }
}
