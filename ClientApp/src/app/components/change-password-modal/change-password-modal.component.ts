import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthentificationService } from 'src/app/services/auth/authentification.service';

@Component({
  selector: 'app-change-password-modal',
  templateUrl: './change-password-modal.component.html',
  styleUrls: ['./change-password-modal.component.css']
})
export class ChangePasswordModalComponent implements OnInit {

  @Output() dismissCallback: EventEmitter<any> = new EventEmitter();

  public passwordForm: FormGroup;
  public successMessage: string = "";
  public errorMessage: string = "";

  constructor(
    private formBuilder: FormBuilder,
    private auth: AuthentificationService
    ) { }

  ngOnInit(): void {
    this.passwordForm = this.formBuilder.group({
      OldPassword: ['', [Validators.required]],
      NewPassword: ['', [Validators.required]],
    });
  }

  dismiss() {
    this.dismissCallback.emit();
  }

  changePassword() {
    if (this.passwordForm.status === 'VALID') {
      this.auth.changePassword(this.passwordForm.value).subscribe(
        (res) => {
          this.errorMessage = "";
          this.successMessage = "Succes";
        },
        (err) => {
          console.log(err);
          if (err.error && err.error.message && err.error.message.length) {
            this.errorMessage = err.error.message;
          } else {
            if (err.error && err.error.errors && err.error.errors.NewPassword.length && err.error.errors.NewPassword[0]) {
              this.errorMessage = err.error.errors.NewPassword[0];
            }
          }
          
          this.successMessage = "";
        }
      );
    }
  }

}
