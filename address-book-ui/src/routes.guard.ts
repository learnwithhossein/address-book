import { Injectable } from '@angular/core';
import { CanActivate, Router, Routes } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from './services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class RoutesGuard implements CanActivate {

    constructor(private auth: AuthService,
        private router: Router, private toastr: ToastrService) { }

    canActivate() {
        const can = this.auth.isLoggedIn();
        if (!can) {
            this.toastr.warning('Please loggin or register first.');
        }

        this.router.navigate(['/register']);

        return can;
    }

}