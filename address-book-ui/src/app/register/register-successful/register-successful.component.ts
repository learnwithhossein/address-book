import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-register-successful',
  templateUrl: './register-successful.component.html',
  styleUrls: ['./register-successful.component.css']
})
export class RegisterSuccessfulComponent implements OnInit {

  firstName;
  email;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.firstName = this.route.snapshot.paramMap.get('firstName');
    this.email = this.route.snapshot.paramMap.get('email');
  }

}

