import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthentificationService } from 'src/app/services/auth/authentification.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup;
  public errorMessage: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private auth: AuthentificationService,
    private router: Router
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
          this.router.navigate(['']);
        },
        (err) => {
          console.log(err);
          this.errorMessage = err.error.message;
        }
      )
    }
  }

}
