import { Component} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserForRegisterDto } from 'src/models/UserForRegisterDto';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
 user = new UserForRegisterDto;
  constructor(private auth :AuthService,
    private toaster : ToastrService) { }
 register =()=>{
   if (this.user.password !== this.user.repeatPassword) {
     this.toaster.error('password and repeat is not equal')
     return;
   }
   this.auth.register(this.user);
 }
 
}
