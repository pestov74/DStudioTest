import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

import { RouterModule, Routes } from '@angular/router';
import { InvoicesComponent } from './pages';
import { InvoiceComponent } from './pages';


const routes: Routes = [
  { path: 'invoices', component: InvoicesComponent },
  { path: 'invoice/:id', component: InvoiceComponent },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class InvoiceRoutingModule { }
