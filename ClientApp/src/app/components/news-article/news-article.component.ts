import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-news-article',
  templateUrl: './news-article.component.html',
  styleUrls: ['./news-article.component.css']
})
export class NewsArticleComponent implements OnInit {

  @Input('title') title: string;
  @Input('source') source: string;
  @Input('imageUrl') imageUrl: string;
  @Input('content') content: string;
  @Input('link') link: string;

  constructor() { }

  ngOnInit(): void {
  }

}
