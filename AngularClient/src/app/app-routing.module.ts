import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {UserProfileFormComponent} from './user-profile/user-profile-form.component';

const routes: Routes = [
  { path: '', redirectTo: '/userProfile', pathMatch: 'full' },
  { path: 'userProfile', component: UserProfileFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
