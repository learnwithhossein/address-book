import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Contact } from 'src/models/contact';
import { map, catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class ApiService {

    constructor(private http: HttpClient, private toastr: ToastrService) { }

    private handleError(error: HttpErrorResponse) {
        console.log(error);
        this.toastr.error(error.error);
        return [];
    }

    private rest = {
        get: (url: string, criteria?: any) => {
            let query = '?'
            if (criteria) {
                for (var property in criteria) {
                    query += `${property}=${criteria[property]}&`;
                }
            }

            return this.http.get(`${environment.apiUrl}${url}${query}`)
                .pipe(map(data => data), catchError(error => this.handleError(error)));
        },
        post: (url: string, body: any) => this.http.post(`${environment.apiUrl}${url}`, body)
            .pipe(map(data => data), catchError(error => this.handleError(error))),
        put: (url: string, body: any) => this.http.put(`${environment.apiUrl}${url}`, body)
            .pipe(map(data => data), catchError(error => this.handleError(error))),
        delete: (url: string, id: number) => {
            const options = {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
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
}
