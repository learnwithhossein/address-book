import { Routes } from '@angular/router';
import { DashboardComponent } from './app/dashboard/dashboard.component';
import { SearchComponent } from './app/search/search.component';
import { RegisterComponent } from './app/register/register.component';
import { NotfoundComponent } from './app/notfound/notfound.component';
import { ContactEditComponent } from './app/contact-edit/contact-edit.component';

export const routes: Routes = [
    { path: 'home', component: DashboardComponent },
    { path: 'search', component: SearchComponent },
    { path: 'edit/:id', component: ContactEditComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'notfound', component: NotfoundComponent },
    { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
