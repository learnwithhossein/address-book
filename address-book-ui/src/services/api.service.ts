import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Contact } from 'src/models/contact';

@Injectable({
    providedIn: 'root'
})
export class ApiService {

    constructor(private http: HttpClient) { }

    private rest = {
        get: (url: string, criteria?: any) => {
            let query = '?'
            if (criteria) {
                for (var property in criteria) {
                    query += `${property}=${criteria[property]}&`;
                }
            }

            return this.http.get(`${environment.apiUrl}${url}${query}`);
        },
        post: (url: string, body: any) => this.http.post(`${environment.apiUrl}${url}`, body),
        put: (url: string, body: any) => this.http.put(`${environment.apiUrl}${url}`, body),
        delete: (url: string, id: number) => {
            const options = {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                body: id
            };

            return this.http.delete(`${environment.apiUrl}${url}`, options);
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
