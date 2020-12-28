import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { AuthentificationService } from '../auth/authentification.service'

@Injectable({
  providedIn: 'root'
})
export class NewsService {
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

  getSources(): Observable<any> {
    return this.http.get(this.url + 'news/sources', this.getHeaders())
  }

  postSource(data: any): Observable<any> {
    return this.http.post(this.url + 'news/sources', data, this.getHeaders())
  }

  deleteSource(id: Number): Observable<any> {
    return this.http.delete(this.url + 'news/sources?id=' + id, this.getHeaders())
  }

  modifySource(data: any): Observable<any> {
    return this.http.put(this.url + 'news/sources', data, this.getHeaders())
  }

  getNews(page: number=0): Observable<any> {
    return this.http.get(this.url + 'news?page=' + page, this.getHeaders())
  }

  // to be deleted
  loadNewNews(): Observable<any> {
    return this.http.get(this.url + 'news/load-new', this.getHeaders());
  }
}
