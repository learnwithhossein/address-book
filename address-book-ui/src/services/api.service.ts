import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Contact } from 'src/models/contact';
import { map, catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { HttpErrors } from '../constants/AddressBook';
import { LoginCredentials } from 'src/models/LoginCredentials';

@Injectable({
    providedIn: 'root'
})
export class ApiService {

    private readonly headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
    });

    private readonly options = {
        headers: this.headers
    };

    constructor(private http: HttpClient, private toastr: ToastrService) { }

    private createQuery(criteria: any) {
        let query = '?'
        if (criteria) {
            for (var property in criteria) {
                query += `${property}=${criteria[property]}&`;
            }
        }

        return query;
    }

    private handleError(error: HttpErrorResponse) {
        console.log(error);

        let message = '';

        if (error.error?.errors) {
            const errors = error.error?.errors;
            for (var errorItem in errors) {
                message += `${errors[errorItem]}<br />`;
            }
        }
        else {
            message = error.error;
        }

        if (!message) {
            message = HttpErrors.find(x => x.code === error.status)?.message;
        }

        this.toastr.error(message);
        return [];
    }

    private rest = {
        get: (url: string, criteria?: any) =>
            this.http.get(`${environment.apiUrl}${url}${this.createQuery(criteria)}`, this.options)
                .pipe(map(data => data), catchError(error => this.handleError(error))),
        post: (url: string, body: any) =>
            this.http.post(`${environment.apiUrl}${url}`, body, this.options)
                .pipe(map(data => data), catchError(error => this.handleError(error))),
        put: (url: string, body: any) =>
            this.http.put(`${environment.apiUrl}${url}`, body, this.options)
                .pipe(map(data => data), catchError(error => this.handleError(error))),
        delete: (url: string, id: number) => {
            const options = {
                headers: this.headers,
                body: id
            };

            return this.http.delete(`${environment.apiUrl}${url}`, options)
                .pipe(map(data => data), catchError(error => this.handleError(error)));
        }
    }

    public contact = {
        getAll: () => this.rest.get(`contact`),
        create: (contact: Contact) => this.rest.post('contact', contact),
        delete: (id: number) => this.rest.delete('contact', id),
        update: (contact: Contact) => this.rest.put('contact', contact),
        getById: (id: number) => this.rest.get(`contact/` + id),
        find: (criteria: any) => this.rest.get(`contact/find`, criteria)
    }

    public auth = {
        login: (body: LoginCredentials) => this.rest.post('auth/login', body)
    }
}
