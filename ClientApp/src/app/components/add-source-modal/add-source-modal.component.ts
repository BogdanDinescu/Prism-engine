import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NewsService } from 'src/app/services/news/news.service';

@Component({
  selector: 'app-add-source-modal',
  templateUrl: './add-source-modal.component.html',
  styleUrls: ['./add-source-modal.component.css']
})
export class AddSourceModalComponent implements OnInit {
  
  @Output() output: EventEmitter<any> = new EventEmitter();
  @Output() dismissCallback: EventEmitter<any> = new EventEmitter();

  public sourceForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private news: NewsService
    ) { }

  ngOnInit(): void {
    this.sourceForm = this.formBuilder.group({
      Name: ['', [Validators.required]],
      Link: ['', [Validators.required]],
    });
  }

  dismiss() {
    this.dismissCallback.emit();
  }

  addSource() {
    if (this.sourceForm.status === 'VALID') {
      this.news.postSource(this.sourceForm.value).subscribe(
        (res) => {
          this.output.emit(res);
        },
        (err) => {
          this.output.emit(err);
        }
      );
    }
  }

}
