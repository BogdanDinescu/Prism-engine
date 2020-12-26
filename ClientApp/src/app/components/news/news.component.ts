import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthentificationService } from 'src/app/services/auth/authentification.service';
import { NewsService } from 'src/app/services/news/news.service';
import { PreferencesService } from 'src/app/services/preferences/preferences.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {

  @Output() loadingChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  public articles: any[];
  public sources: any[];
  public loadingMore: boolean = false;
  public noSources: boolean;
  private page: number = 0;

  constructor(
    private news: NewsService,
    private preferences: PreferencesService,
    private auth: AuthentificationService
    ) { }

  ngOnInit(): void {
    this.news.getSources().subscribe(
      (res) => {
        this.sources = res;
        this.noSources = this.selectedSourcesIds().length === 0;
      },
      (err) => {
        console.log(err);
      }
    )
    this.news.getNews().subscribe(
      (res) => {
        this.articles = res.news;
        this.updateLoading(false);
      },
      (err) => {
        console.log(err);
      }
    );
  }

  updateLoading(loading: boolean) {
    console.log(loading);
    this.loadingChange.emit(loading);
  }

  sourceClick(sourceName:string): void {
    let foundSource = this.sources.find(source => source.name === sourceName);
    if (foundSource) {
      foundSource.selected = !foundSource.selected;
    }
  }

  selectedSourcesIds(): Array<Number> {
    return this.sources.filter(source => source.selected).map(source => source.id);
  }

  updateSources(): void {
    this.updateLoading(true);
    let ids = this.selectedSourcesIds();
    this.preferences.setNewsPreferences(ids).subscribe(
      (res) => {
        this.news.getNews().subscribe(
          (res) => {
            this.articles = res.news;
            this.noSources = this.selectedSourcesIds().length === 0;
            this.updateLoading(false);
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

  loadMore(): void {
    this.loadingMore = true;
    this.page = this.page + 1;
    this.news.getNews(this.page).subscribe(
      (res) => {
        console.log(res)
        this.articles = this.articles.concat(res.news);
        this.loadingMore = false;
      },
      (err) => {
        console.log(err);
      }
    )
  }

}
