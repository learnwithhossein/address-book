import { Component, OnDestroy, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/services/auth.service';

@Component({
    selector: 'app-index',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {

    hubConnection: HubConnection;

    constructor(private auth: AuthService, private toastr: ToastrService) { }

    ngOnInit(): void {
        this.connect();
    }
    ngOnDestroy(): void {
        this.disconnect();
    }

    connect = () => {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.signalRUrl, {
                accessTokenFactory: () => this.auth.getToken()
            })
            .withAutomaticReconnect()
            .build();

        this.hubConnection.on('LoginEven', data => {
            const { userId, message } = data;
            this.toastr.info(message);
        });

        this.hubConnection.on('ImageUploadEvent', data => {
            const { contactId } = data;
            this.toastr.info(`Contact with Id ${contactId} received an image update.`);
        });

        this.hubConnection.start();
    }

    disconnect = () => {
        this.hubConnection.stop();
    }
}
