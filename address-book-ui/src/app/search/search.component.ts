import { Component, OnInit } from '@angular/core';
import {ApiService} from 'src/services/api.service';
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  rowData;
  constructor(private api : ApiService ) { }

  ngOnInit(): void {
    this.search('')
;  }
  columnDefs = [
    { field: 'id'  },
    { field: 'firstName' ,sortable: true,filter: true},
    { field: 'lastName'},
    { field: 'tellNo'},
    { field: 'cellNo'}
];

search =(name)=>{
  this.api.contact.search(name).subscribe(data=>{
    this.rowData = data;
    
  });
}

}
