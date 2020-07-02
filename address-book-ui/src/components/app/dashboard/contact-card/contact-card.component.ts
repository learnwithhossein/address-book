import { Component, Input } from '@angular/core';
import { Contact } from 'src/models/contact';

@Component({
    selector: 'app-contact-card',
    templateUrl: './contact-card.component.html',
    styleUrls: ['./contact-card.component.css']
})
export class ContactCardComponent {

    @Input() contact: Contact;

}
