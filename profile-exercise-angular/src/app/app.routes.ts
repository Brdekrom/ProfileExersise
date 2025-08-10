// src/app/app.routes.ts
import { Routes } from '@angular/router';
import { ProfileListComponent } from './profile/profile-list.component/profile-list.component';
import { ProfileEditComponent } from './profile/profile-edit.component/profile-edit.component';


export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'profiles' },
  { path: 'profiles', component: ProfileListComponent },
  { path: 'profiles/new', component: ProfileEditComponent },
  { path: 'profiles/:id', component: ProfileEditComponent },
];
