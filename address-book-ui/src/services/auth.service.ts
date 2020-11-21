import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, Subscription } from 'rxjs';
import { LoginResult } from 'src/models/LoginResult';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private subscription: Subscription;

    private behaviour = new BehaviorSubject(false);
    currentStatus = this.behaviour.asObservable();

    private loadinBehaviour = new BehaviorSubject(false);
    loadingStatus = this.loadinBehaviour.asObservable();

    constructor(private api: ApiService, private router: Router) { }

    changeLoginStatus(status: boolean) {
        this.behaviour.next(status)
    }

    changeLoadingStatus(status: boolean) {
        this.loadinBehaviour.next(status)
    }

    login = (credentials) => {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }

        this.changeLoadingStatus(true);

        this.subscription = this.api.auth.login(credentials)
            .subscribe((result: LoginResult) => {
                localStorage.setItem('jwtToken', result.jwtToken);
                localStorage.setItem('firstName', result.firstName);
                localStorage.setItem('id', result.id);

                this.changeLoginStatus(true);

                this.changeLoadingStatus(false);
            }, () => {
                this.changeLoadingStatus(false);
            });
    }

    update = (user) => {
        this.api.auth.update(user);
    }

    logout = () => {
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('id');
        localStorage.removeItem('firstName');
        this.changeLoginStatus(false);
    }

    getFirstName = () => localStorage.getItem('firstName');

    getId = () => localStorage.getItem('id');

    getUser = () => this.api.auth.get(this.getId());

    isLoggedIn = () => {
        const token = this.getToken();
        if (!token) {
            return false;
        }

        const { exp } = this.parseJwt(token);
        const now = Date.now() / 1000;

        return now < exp;
    };

    getToken = () => localStorage.getItem('jwtToken');

    register = (user) => {
        this.api.auth.register(user)
            .subscribe(data => {
                const info: { email, firstName } = data as any;
                this.router.navigate(['/register-successful', info]);
            });
    }

    confirm = (id) => this.api.auth.confirm({ id });

    private parseJwt(token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    };
}
