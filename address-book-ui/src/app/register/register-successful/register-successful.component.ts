import { Component, OnInit } from '@angular/core';
import { ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-register-successful',
  templateUrl: './register-successful.component.html',
  styleUrls: ['./register-successful.component.css']
})
export class RegisterSuccessfulComponent implements OnInit {

  constructor(private route : ActivatedRoute) { }
firstName;
email;
  ngOnInit(): void {
    this.firstName = this.route.snapshot.paramMap.get('firstName');
    this.email = this.route.snapshot.paramMap.get('email');

  }

}
