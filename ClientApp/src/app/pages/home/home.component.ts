import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
 
  public loading: boolean;

  constructor(
    private router: Router,
    ) { }

  ngOnInit(): void {

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
