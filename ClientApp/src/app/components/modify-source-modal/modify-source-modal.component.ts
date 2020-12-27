import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NewsService } from 'src/app/services/news/news.service';

@Component({
  selector: 'app-modify-source-modal',
  templateUrl: './modify-source-modal.component.html',
  styleUrls: ['./modify-source-modal.component.css']
})
export class ModifySourceModalComponent implements OnInit {

  @Input() source: any;
  @Output() modifiedSource: EventEmitter<any> = new EventEmitter();
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
      this.sourceForm.value.id = this.source.id;
      this.news.modifySource(this.sourceForm.value).subscribe(
        (res) => {
          this.modifiedSource.emit(res);
        },
        (err) => {
          this.dismissCallback.emit();
        }
      );
    }
  }
}
