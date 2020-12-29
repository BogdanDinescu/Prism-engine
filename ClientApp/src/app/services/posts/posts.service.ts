import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthentificationService } from '../auth/authentification.service';

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  private url: string = environment.url;
  constructor(private http: HttpClient, private auth: AuthentificationService) { }

  private getHeaders() {
    return {
      headers: new HttpHeaders({
        'content-type': 'application/json',
        'authorization': 'Bearer ' + this.auth.getToken(),
      })
    };
  }

  getPosts(): Observable<any> {
    return this.http.get(this.url + 'post', this.getHeaders())
  }

  postPost(data: any): Observable<any> {
    return this.http.post(this.url + 'post', data, this.getHeaders());
  }

  putPost(data: any): Observable<any> {
    return this.http.put(this.url + 'post', data, this.getHeaders());
  }

  deletePost(id: Number): Observable<any> {
    return this.http.delete(this.url + 'post?id=' + id, this.getHeaders());
  }

}
