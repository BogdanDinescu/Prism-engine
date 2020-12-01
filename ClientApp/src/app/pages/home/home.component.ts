import { Component, OnInit } from '@angular/core';
import { NewsService } from "../../services/news/news.service"

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public articles: [any];
  constructor(private news: NewsService) { }

  ngOnInit(): void {
    this.news.getNews().subscribe(
      (res) => {
        this.articles = res.news;
      },
      (err) => {
        console.log(err)
      }
    );
  }
}
