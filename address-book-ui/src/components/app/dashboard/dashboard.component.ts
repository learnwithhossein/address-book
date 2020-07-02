import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/services/api.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    constructor(private api: ApiService) { }

    ngOnInit(): void {
        const criteria = {
            address: '1',
            name: 'ali'
        };

        this.api.contact.find(criteria).subscribe(data => {
            this.contacts = data;
        });
    }

    contacts;
    title = 'address-book-ui';

}
