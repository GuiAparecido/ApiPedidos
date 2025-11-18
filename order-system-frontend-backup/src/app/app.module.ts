import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { ProductsComponent } from './components/products/products.component';
import { ClientsComponent } from './components/clients/clients.component';
import { OrdersComponent } from './components/orders/orders.component';
import { CreateOrderComponent } from './components/create-order/create-order.component';

import { ApiService } from './services/api.service';

@NgModule({
  declarations: [
    AppComponent,
    ProductsComponent,
    ClientsComponent,
    OrdersComponent,
    CreateOrderComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    RouterModule
  ],
  providers: [ApiService],
  bootstrap: [AppComponent]
})
export class AppModule {}
