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
  public errorMessage: string = '';
  public success: boolean = false;
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
    if (this.registerForm.status === 'VALID') {
      this.auth.register(this.registerForm.value).subscribe(
        (res) => {
          this.success = true;
          this.errorMessage = "";
        },
        (err) => {
          if (err.error && err.error.errors) {
            if (err.error.errors.Email) {
              this.errorMessage = err.error.errors.Email[0];
              return;
            }
            if (err.error.errors.Password) {
              this.errorMessage = err.error.errors.Password[0];
              return;
            }
            if (err.error.errors.Name) {
              this.errorMessage = err.error.errors.Name[0];
              return;
            }
          }
            
        }
      );
    }
  }

}
