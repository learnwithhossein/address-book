import { Component } from '@angular/core';
import { AuthService } from 'src/services/auth.service';
import { LoginCredentials } from 'src/models/LoginCredentials';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

    credentials = new LoginCredentials;

    constructor(private auth: AuthService) { }

    login = () => {
        this.auth.login(this.credentials);
    }

    getFirstName = () => this.auth.getFirstName();

    logout = () => this.auth.logout();

    isLoggedIn = () => this.auth.isLoggedIn();
}
