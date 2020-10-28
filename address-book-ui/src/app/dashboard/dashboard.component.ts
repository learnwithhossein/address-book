import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/services/api.service';
import { Contact } from 'src/models/contact';
import { AuthService } from 'src/services/auth.service';
import { NgxSpinnerService } from "ngx-spinner";
import { Pagination } from 'src/models/Pagination';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    contacts: Contact[];
    showDashboard = false;
    pagination: Pagination;
    radioModel = '8';
    orderBy='FirstName';
    sort ='asc';
    criteria;
    constructor(
        private api: ApiService,
        private auth: AuthService,
        private spinner: NgxSpinnerService) { }

    ngOnInit(): void {
        const criteriaString=localStorage.getItem('criteria');
        if ( criteriaString) {
            this.criteria=  JSON.parse(localStorage.getItem('criteria'));
            this.sort=this.criteria.sort;
            this.orderBy=this.criteria.orderBy;
            this.pagination =new Pagination;
            this.pagination.pageSize=this.criteria.pageSize;
            this.pagination.pageNumber=this.criteria.pageNumber;
            this.pagination.totalCount=this.criteria.totalCount;
            this.radioModel= this.pagination.pageSize.toString();
        } else {
            this.criteria={
                ...this.pagination,
                sort : this.sort,
                orderBy : this.orderBy,
            }
        }
        this.criteria=localStorage.getItem('criteria') ?
         JSON.parse(localStorage.getItem('criteria')):{};
        this.auth.loadingStatus.subscribe(data => {
            if (data) {
                this.spinner.show();
            }
            else {
                this.spinner.hide();
            }
        })

        this.auth.currentStatus.subscribe(data => {
            if (data) {
                this.loadData();
            }
            else {
                this.showDashboard = false;
            }
        });

        if (this.auth.isLoggedIn()) {
            this.loadData();
        }
    }

    pageChanged = (params) => {
        const { page, itemsPerPage } = params;

        this.pagination.pageNumber = page;
        this.pagination.pageSize = itemsPerPage;
        this.criteria={
            ...this.pagination,
            sort : this.sort,
            orderBy : this.orderBy,
        }

        this.loadData();
    }
    
    loadData = () => {
       
        localStorage.setItem('criteria', JSON.stringify(this.criteria));

        this.api.contact.find(this.criteria).subscribe(data => {
            this.contacts = data.body as Contact[];
            this.pagination = data.pagination;
        });

        this.showDashboard = true;
    }

    deleteContact = (id: number) => {
        const index = this.contacts.findIndex(x => x.id === id);
        this.contacts.splice(index, 1);
    }
    pageSizeChanged=()=>{
        this.pagination.pageSize= +this.radioModel;
        this.criteria={
            ...this.pagination,
            sort : this.sort,
            orderBy : this.orderBy,
        }
        this.loadData();
    }
    changeOrderBy =(propertyName)=>{
            this.orderBy=propertyName;
            this.criteria={
                ...this.pagination,
                sort : this.sort,
                orderBy : this.orderBy,
            }
            this.loadData();
    }
    ChangSort=()=>{
        this.criteria={
            ...this.pagination,
            sort : this.sort,
            orderBy : this.orderBy,
        }
        this.loadData();
    }
}
