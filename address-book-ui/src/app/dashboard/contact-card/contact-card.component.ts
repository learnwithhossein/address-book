import { Component, Input, TemplateRef, Output, EventEmitter } from '@angular/core';
import { Contact } from 'src/models/contact';
import { ApiService } from 'src/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    selector: 'app-contact-card',
    templateUrl: './contact-card.component.html',
    styleUrls: ['./contact-card.component.css']
})
export class ContactCardComponent {

    @Input() contact: Contact;
    @Output() contactDeleted = new EventEmitter<number>();

    modalRef: BsModalRef;

    constructor(
        private api: ApiService,
        private toastr: ToastrService,
        private modalService: BsModalService) { }

    openModal(template: TemplateRef<any>) {
        this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
    }

    confirm(): void {
        this.api.contact.delete(this.contact.id)
            .subscribe(() => {
                this.toastr.info(`${this.contact.firstName} ${this.contact.lastName} was deleted.`);
                this.contactDeleted.emit(this.contact.id);
            });

        this.modalRef.hide();
    }

    decline(): void {
        this.modalRef.hide();
    }
}
