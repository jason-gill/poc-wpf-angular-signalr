import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ContractFormComponent} from './contract-form/contract-form.component';

const routes: Routes = [
  { path: '', redirectTo: '/contract', pathMatch: 'full' },
  { path: 'contract', component: ContractFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
