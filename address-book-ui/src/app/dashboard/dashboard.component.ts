import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/services/api.service';
import { Contact } from 'src/models/contact';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    contacts: Contact[];

    constructor(private api: ApiService) { }

    ngOnInit(): void {
        const criteria = {
            name: 'ali'
        };

        this.api.contact.find(criteria).subscribe(data => {
            this.contacts = data as Contact[];
        });
    }

    deleteContact = (id: number) => {
        const index = this.contacts.findIndex(x => x.id === id);
        this.contacts.splice(index, 1);
    }
}
