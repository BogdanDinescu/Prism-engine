import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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

  public sourceForm: FormGroup;
  public articles: any[];
  public sources: any[];
  public loadingMore: boolean = false;
  public noSources: boolean;
  private page: number = 0;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private news: NewsService,
    private preferences: PreferencesService,
    public auth: AuthentificationService
    ) { }

    open(content) {
      this.modalService.open(content);  
    }

  ngOnInit(): void {
    this.news.getSources().subscribe(
      (res) => {
        this.sources = res;
        this.noSources = this.selectedSourcesIds().length === 0;
      },
      (err) => {
        console.log(err);
      }
    );
    this.news.getNews().subscribe(
      (res) => {
        this.articles = res.news;
        this.updateLoading(false);
      },
      (err) => {
        console.log(err);
      }
    );
    this.sourceForm = this.formBuilder.group({
      Name: ['', [Validators.required]],
      Link: ['', [Validators.required]],
    });
  }

  updateLoading(loading: boolean) {
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

  addSource() {
    if (this.sourceForm.status === 'VALID') {
      this.news.postSource(this.sourceForm.value).subscribe(
        (res) => {
          this.modalService.dismissAll();
          this.news.getSources().subscribe(
            (res) => {
              this.sources = res;
              this.noSources = this.selectedSourcesIds().length === 0;
            },
            (err) => {
              console.log(err);
            }
          );
        },
        (err) => {
          console.log(err);
        }
      );
    }
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
