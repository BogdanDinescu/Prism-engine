import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthentificationService } from '../auth/authentification.service';

@Injectable({
  providedIn: 'root'
})
export class PreferencesService {
  private url: string = environment.url;
  constructor(private http: HttpClient, private auth: AuthentificationService) {}

  private getHeaders() {
    return {
      headers: new HttpHeaders({
        'content-type': 'application/json',
        'authorization': 'Bearer ' + this.auth.getToken(),
      })
    };
  }

  setNewsPreferences(data: any): Observable<any> {
    return this.http
      .post(this.url + 'preferences/news', data, this.getHeaders())
  }
}
