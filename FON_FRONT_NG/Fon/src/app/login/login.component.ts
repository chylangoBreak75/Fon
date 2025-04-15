import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  formLogin: FormGroup;
  error: string = "";

  constructor(private readonly fb: FormBuilder,
    private readonly loginService: LoginService,
    private readonly router: Router
  ) {
    this.formLogin = this.fb.group({
      uname: [null, [Validators.required]],
      password: [null, [Validators.required]]
    });
  }

  get f() { return this.formLogin.controls; }

  onClickLogin(loginEvent: any) {
      console.log(loginEvent);
      const userForm = {
          name: this.f["uname"].value,
          pwd: this.f["password"].value
      };
      console.log(userForm);
      this.loginService.userLogin(userForm).subscribe({
        next:(response) => {
          console.log(response);
          if(response.result == "Accepted") {
            this.router.navigate(["/list"]);
          }
          if(response.result == "Not Accepted") {
            alert('usr/pwd Not Accepted')
          }
        },
        error:(error) => { console.log(error); this.error = error; }
      });
  }

}
