import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthentificationService } from 'src/app/services/auth/authentification.service';
import { NewsService } from 'src/app/services/news/news.service';
import { PreferencesService } from 'src/app/services/preferences/preferences.service';
import { AddSourceModalComponent } from 'src/app/components/add-source-modal/add-source-modal.component';
import { ModifySourceModalComponent } from '../modify-source-modal/modify-source-modal.component';

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
    private modalService: NgbModal,
    private news: NewsService,
    private preferences: PreferencesService,
    public auth: AuthentificationService
    ) { }

  openAddSourceModal() {
    const modalRef = this.modalService.open(AddSourceModalComponent);
    modalRef.componentInstance.output.subscribe(
      (res: any) => {
        if (res) {
          console.log(res.message);
        } else {
          this.getSources();
          modalRef.close();
        }
      }
    )
    modalRef.componentInstance.dismissCallback.subscribe(
        () => {
          modalRef.dismiss();
        }
    )
  }

  openModifySourceModal(source: any) {
    const modalRef = this.modalService.open(ModifySourceModalComponent);
    modalRef.componentInstance.source = source;
    modalRef.componentInstance.modifiedSource.subscribe(
      (res: any) => {
        let index = this.sources.findIndex(source => source.id === res.id);
        this.sources[index].name = res.name;
        this.sources[index].link = res.link;
        modalRef.close();
      }
    )
    modalRef.componentInstance.dismissCallback.subscribe(
      () => {
        modalRef.dismiss();
      }
    )
  }

  ngOnInit(): void {
    this.getSources();
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

  getSources() {
    this.news.getSources().subscribe(
      (res) => {
        this.sources = res;
        this.noSources = this.selectedSourcesIds().length === 0;
      },
      (err) => {
        console.log(err);
      }
    );
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

  deleteSource(id: Number) {;
    this.news.deleteSource(id).subscribe(
      (res) => {
        this.getSources()
      },
      (err) => {
        console.log(err);
      }
    );
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
