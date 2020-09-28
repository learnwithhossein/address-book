import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Contact } from 'src/models/contact';
import { ApiService } from 'src/services/api.service';

@Component({
    selector: 'app-contact-edit',
    templateUrl: './contact-edit.component.html',
    styleUrls: ['./contact-edit.component.css']
})
export class ContactEditComponent implements OnInit {

    contact: Contact;

    constructor(private api: ApiService,
        private route: ActivatedRoute,
        private location: Location) { }

    ngOnInit(): void {
        const id = +this.route.snapshot.paramMap.get('id');
        console.log(id)
        this.api.contact.getById(id).subscribe(data => {
            this.contact = data as Contact;
            console.log(this.contact);
        });
    }

    save = () => {
        this.api.contact.update(this.contact).subscribe();
        this.location.back();
    }

}
