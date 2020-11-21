import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthentificationService } from 'src/app/services/authentification.service'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private auth: AuthentificationService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      Email: ['',[Validators.required, Validators.email]],
      Password: ['',[Validators.required]]
    });
  }

  doLogin() {
    if (this.loginForm.status === 'VALID') {
      this.auth.login(this.loginForm.value).subscribe(
        (res) => {
          console.log(res);
        },
        (err) => {
          console.log(err)
        }
      )
    }
  }

}
