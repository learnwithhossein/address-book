import { Routes } from '@angular/router';
import { DashboardComponent } from './app/dashboard/dashboard.component';
import { SearchComponent } from './app/search/search.component';
import { RegisterComponent } from './app/register/register.component';
import { NotfoundComponent } from './app/notfound/notfound.component';
import { ContactEditComponent } from './app/contact-edit/contact-edit.component';
import { RegisterSuccessfulComponent } from './app/register/register-successful/register-successful.component';
import { RegisterConfirmedComponent } from './app/register/register-confirmed/register-confirmed.component';

export const routes: Routes = [
    { path: 'home', component: DashboardComponent },
    { path: 'search', component: SearchComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'register-successful', component: RegisterSuccessfulComponent },
    { path: 'register-confirmed', component: RegisterConfirmedComponent },
    { path: 'notfound', component: NotfoundComponent },
    { path: 'edit/:id', component: ContactEditComponent },
    { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
