import { Component } from '@angular/core';
import { AuthService } from 'src/services/auth.service';
import { LoginCredentials } from 'src/models/LoginCredentials';
import { Router } from '@angular/router';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

    credentials = new LoginCredentials;

    constructor(private auth: AuthService, private router: Router) { }

    login = () => {
        this.auth.login(this.credentials);

        this.router.navigate(['/home']);
    }

    getFirstName = () => this.auth.getFirstName();

    logout = () => {
        this.auth.logout();

        this.router.navigate(['/register']);
    }

    isLoggedIn = () => this.auth.isLoggedIn();
}
