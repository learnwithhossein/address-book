import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/services/api.service';
import { Contact } from 'src/models/contact';
import { AuthService } from 'src/services/auth.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    contacts: Contact[];
    showDashboard = false;

    constructor(private api: ApiService, private auth: AuthService) { }

    ngOnInit(): void {
        this.auth.currentStatus.subscribe(data => {
            if (data) {
                const criteria = {
                    name: 'ali'
                };

                this.api.contact.find(criteria).subscribe(data => {
                    this.contacts = data as Contact[];
                });

                this.showDashboard = true;
            }
            else {
                this.showDashboard = false;
            }
        });
    }

    deleteContact = (id: number) => {
        const index = this.contacts.findIndex(x => x.id === id);
        this.contacts.splice(index, 1);
    }
}
