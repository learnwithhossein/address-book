import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserForRegisterDto } from 'src/models/UserForRegisterDto';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  user = new UserForRegisterDto();

  constructor(private auth: AuthService, private toastr: ToastrService) { }

  register = () => {
    if (this.user.password !== this.user.repeatPassword) {
      this.toastr.error('Password and repeat are not equal.');
      return;
    }

    this.auth.register(this.user);
  }
}
