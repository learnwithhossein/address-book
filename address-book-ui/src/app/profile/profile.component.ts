import { Component, OnInit, ViewChild } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { environment } from 'src/environments/environment';
import { ApiService } from 'src/services/api.service';
import { AuthService } from 'src/services/auth.service';
import { User } from 'src/models/User';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  @ViewChild('staticTabs', { static: false }) staticTabs: TabsetComponent;

  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  user: User;

  constructor(private api: ApiService,
    private auth: AuthService) { }

  fileOverBase = (params) => {
    this.hasBaseDropZoneOver = params;
  }

  ngOnInit(): void {
    const id = this.auth.getId();

    this.auth.getUser()
      .subscribe(data => {
        this.user = data as User;

        this.uploader = new FileUploader({
          allowedFileType: ['image'],
          authToken: `Bearer ${this.auth.getToken()}`,
          autoUpload: false,
          isHTML5: true,
          maxFileSize: 5 * 1024 * 1024,
          queueLimit: 1,
          removeAfterUpload: true,
          url: `${environment.apiUrl}auth/${id}/image`
        });

        this.uploader.onSuccessItem = (item, response, status, headers) => {
          if (response && status === 200) {
            const { imageUrl } = JSON.parse(response);
            this.user.imageUrl = imageUrl;
            this.staticTabs.tabs[0].active = true;
          }
        }
      });
  }

  save = () => {
    this.api.auth.update(this.user).subscribe();
  }

}
