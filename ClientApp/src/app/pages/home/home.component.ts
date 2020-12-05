import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PreferencesService } from 'src/app/services/preferences/preferences.service';
import { NewsService } from "../../services/news/news.service"

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public articles: any[];
  public sources: any[];
  public loading: boolean = true;
  constructor(
    private news: NewsService,
    private preferences: PreferencesService,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.news.getSources().subscribe(
      (res) => {
        this.sources = res;
      },
      (err) => {
        console.log(err);
      }
    )
    this.news.getNews().subscribe(
      (res) => {
        this.articles = res.news;
        this.loading = false;
      },
      (err) => {
        console.log(err);
      }
    );
  }

  sourceClick(sourceName:string): void {
    let foundSource = this.sources.find(source => source.name === sourceName);
    if (foundSource) {
      foundSource.selected = !foundSource.selected;
    }
  }

  updateSources(): void {
    this.loading = true;
    let ids = this.sources.filter(source => source.selected).map(source => source.id);
    this.preferences.setNewsPreferences(ids).subscribe(
      (res) => {

        this.news.getNews().subscribe(
          (res) => {
            this.articles = res.news;
            this.loading = false;
          },
          (err) => {
            console.log(err);
          }
        );

      },
      (err) => {
        console.log(err);
      }
    )
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login'])
  }
}
