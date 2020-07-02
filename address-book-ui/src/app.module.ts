import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './components/app/app.component';
import { DashboardComponent } from './components/app/dashboard/dashboard.component';
import { ContactCardComponent } from './components/app/dashboard/contact-card/contact-card.component';

@NgModule({
    declarations: [
        AppComponent,
        DashboardComponent,
        ContactCardComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
