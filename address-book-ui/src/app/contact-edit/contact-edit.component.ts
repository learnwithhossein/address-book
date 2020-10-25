import { Location } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { environment } from 'src/environments/environment';
import { Contact } from 'src/models/contact';
import { ApiService } from 'src/services/api.service';
import { AuthService } from 'src/services/auth.service';

@Component({
    selector: 'app-contact-edit',
    templateUrl: './contact-edit.component.html',
    styleUrls: ['./contact-edit.component.css']
})
export class ContactEditComponent implements OnInit {

    @ViewChild('staticTabs', { static: false }) staticTabs: TabsetComponent;

    uploader: FileUploader;
    hasBaseDropZoneOver = false;
    contact: Contact;

    constructor(private api: ApiService,
        private route: ActivatedRoute,
        private location: Location,
        private auth: AuthService) { }

    fileOverBase = (params) => {
        this.hasBaseDropZoneOver = params;
    }

    ngOnInit(): void {
        const id = +this.route.snapshot.paramMap.get('id');
        this.api.contact.getById(id).subscribe(data => {
            this.contact = data as Contact;
        });

        this.uploader = new FileUploader({
            allowedFileType: ['image'],
            authToken: `Bearer ${this.auth.getToken()}`,
            autoUpload: false,
            isHTML5: true,
            maxFileSize: 5 * 1024 * 1024,
            queueLimit: 1,
            removeAfterUpload: true,
            url: `${environment.apiUrl}contact/${id}/image`
        });

        this.uploader.onSuccessItem = (item, response, status, headers) => {
            if (response && status === 200) {
                const { imageUrl } = JSON.parse(response);
                this.contact.imageUrl = imageUrl;
                this.staticTabs.tabs[0].active = true;
            }
        }
    }

    save = () => {
        this.api.contact.update(this.contact).subscribe();
        this.location.back();
    }

}
