import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
 
  public loading: boolean;
  public open: string;

  constructor(
    private router: Router,
    ) { }

  ngOnInit(): void {
    let tab = localStorage.getItem("tab");
    if(tab) {
      this.open = tab;
    } else {
      this.open = "news";
    }

  }

  doOpen(content: string):void {
    this.open = content;
    localStorage.setItem("tab",content);
  }

  update(loading: boolean) {
    this.loading = loading;
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login'])
  }

  settings() {
    this.router.navigate(['/settings'])
  }
}
