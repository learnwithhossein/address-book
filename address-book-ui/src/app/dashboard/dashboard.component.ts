import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/services/api.service';
import { Contact } from 'src/models/contact';
import { AuthService } from 'src/services/auth.service';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    contacts: Contact[];
    showDashboard = false;

    constructor(
        private api: ApiService,
        private auth: AuthService,
        private spinner: NgxSpinnerService) { }

    ngOnInit(): void {
        this.auth.loadingStatus.subscribe(data => {
            if (data) {
                this.spinner.show();
            }
            else {
                this.spinner.hide();
            }
        })

        this.auth.currentStatus.subscribe(data => {
            if (data) {
                this.loadData();
            }
            else {
                this.showDashboard = false;
            }
        });

        if (this.auth.isLoggedIn()) {
            this.loadData();
        }
    }

    loadData = () => {
        const criteria = {
            name: 'ali'
        };

        this.api.contact.find(criteria).subscribe(data => {
            this.contacts = data as Contact[];
        });

        this.showDashboard = true;
    }

    deleteContact = (id: number) => {
        const index = this.contacts.findIndex(x => x.id === id);
        this.contacts.splice(index, 1);
    }
}
