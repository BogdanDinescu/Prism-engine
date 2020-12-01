import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AuthentificationService } from '../auth/authentification.service'

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  private url: string = environment.url;
  constructor(private http: HttpClient, private auth: AuthentificationService) {}

  getHeaders() {
    return {
      headers: new HttpHeaders({
        'content-type': 'application/json',
        'authorization': 'Bearer ' + this.auth.getToken(),
      })
    };
  }

  getNews(): Observable<any> {
    return this.http
      .get(this.url + 'news?links=http://rss.realitatea.net/stiri.xml', this.getHeaders());
  }
}
