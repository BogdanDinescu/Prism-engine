import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordModalComponent } from 'src/app/components/change-password-modal/change-password-modal.component';
import { User } from 'src/app/models/User';
import { AuthentificationService } from 'src/app/services/auth/authentification.service';


@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})

export class SettingsComponent implements OnInit {

  public user: User = new User();
  userForm: FormGroup;
  success: boolean = false;
  errorMessage: string = "";

  constructor(
    private router: Router,
    public auth: AuthentificationService,
    private formBuilder: FormBuilder,
    private modalService: NgbModal
    ) { }

  getUser() {
    this.auth.getUser().subscribe(
      (res) => {
        this.user = res;
      }
    )
  }
  
  ngOnInit(): void {
    this.getUser()
    this.userForm = this.formBuilder.group({
      Name: ['', [Validators.required]],
    });
  }
  
  openChangePasswordModal() {
    const modalRef = this.modalService.open(ChangePasswordModalComponent);
    modalRef.componentInstance.dismissCallback.subscribe(
        () => {
          modalRef.close();
        }
    )
  }

  doChange() {
    if (this.userForm.status === 'VALID') {
      this.auth.updateUser(this.userForm.value).subscribe(
        (res) => {
          this.success = true;
          this.errorMessage = "";
          this.user = res;
        },
        (err) => {
          this.success = false;
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

  deleteUser() {
    if(confirm("Contul va fi șters și toate datele asociate lui. Sunteți sigur?")) {
      this.auth.deleteUser().subscribe(
        (res) => {
          localStorage.clear();
          this.router.navigate(['/login']);
        },
        (err) => {
          console.log(err);
        }
      )
    }
  }

}
