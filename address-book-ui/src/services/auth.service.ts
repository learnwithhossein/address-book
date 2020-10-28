import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, Subscription } from 'rxjs';
import { LoginResult } from 'src/models/LoginResult';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private subscription: Subscription;

    private behaviour = new BehaviorSubject(false);
    currentStatus = this.behaviour.asObservable();

    private loadinBehaviour = new BehaviorSubject(false);
    loadingStatus = this.loadinBehaviour.asObservable();

    constructor(private api: ApiService) { }

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

                this.changeLoginStatus(true);

                this.changeLoadingStatus(false);
            }, () => {
                this.changeLoadingStatus(false);
            });
    }

    logout = () => {
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('firstName');
        this.changeLoginStatus(false);
    }

    getFirstName = () => localStorage.getItem('firstName');

    isLoggedIn = () => localStorage.getItem('jwtToken');
    getToken =()=>localStorage.getItem('jwtToken');
   
}
