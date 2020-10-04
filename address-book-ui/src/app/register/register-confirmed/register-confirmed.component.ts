import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-register-confirmed',
  templateUrl: './register-confirmed.component.html',
  styleUrls: ['./register-confirmed.component.css']
})
export class RegisterConfirmedComponent implements OnInit {

  firstName;
  email;

  constructor(private route: ActivatedRoute, private auth: AuthService) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.auth.confirm(id).subscribe(data => {
      const { firstName, email } = data as any;

      this.firstName = firstName;
      this.email = email;
    });
  }

}
