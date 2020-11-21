import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthentificationService } from 'src/app/services/auth/authentification.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public registerForm: FormGroup;
  public submitted: boolean = false;
  public serverError: boolean = false;
  constructor(
    private formBuilder: FormBuilder,
    private auth: AuthentificationService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      Email: ['', [Validators.required, Validators.email]],
      Name: ['', [Validators.required]],
      Password: ['', [Validators.required]],
      ConfirmPassword: ['', [Validators.required]],
    }, {validator: this.passwordConfirming('Password', 'ConfirmPassword')});
  }
  passwordConfirming(Password: string, ConfirmPassword: string) {
    return (group: FormGroup) => {
      const input = group.controls[Password];
      const confirmationInput = group.controls[ConfirmPassword];
      return confirmationInput.setErrors(
          input.value !== confirmationInput.value ? {notEquivalent: true} : null
      );
    };
  }

  doRegister() {
    this.submitted = true;
    this.serverError = false;

    if (this.registerForm.status === 'VALID') {
      this.auth.register(this.registerForm.value).subscribe(
        (res) => {
          console.log(res);
        },
        (err) => {
          if (err) {
            this.serverError = true;
          }
        }
      );
    }
  }

}
