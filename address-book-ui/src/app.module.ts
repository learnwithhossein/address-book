import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from "ngx-spinner";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { AppComponent } from './app/app.component';
import { DashboardComponent } from './app/dashboard/dashboard.component';
import { ContactCardComponent } from './app/dashboard/contact-card/contact-card.component';
import { NavbarComponent } from './app/navbar/navbar.component';
import { SearchComponent } from './app/search/search.component';
import { RegisterComponent } from './app/register/register.component';
import { NotfoundComponent } from './app/notfound/notfound.component';
import { RouterModule } from '@angular/router';
import { routes } from './routes';
import { ContactEditComponent } from './app/contact-edit/contact-edit.component';

@NgModule({
    declarations: [
        AppComponent,
        DashboardComponent,
        ContactCardComponent,
        NavbarComponent,
        SearchComponent,
        RegisterComponent,
        NotfoundComponent,
        ContactEditComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot({ enableHtml: true }),
        ModalModule.forRoot(),
        BsDropdownModule.forRoot(),
        PaginationModule.forRoot(),
        ButtonsModule.forRoot(),
        RouterModule.forRoot(routes),
        BrowserModule,
        NgxSpinnerModule,
        HttpClientModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
