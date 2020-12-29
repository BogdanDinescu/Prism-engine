import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Post } from 'src/app/models/Post';
import { PostsService } from 'src/app/services/posts/posts.service';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {

  @Output() loadingChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  public posts: Array<Post> = [];
  public postForm: FormGroup;
  public errorMessage: string = "";
  public alertMessage: string = "This is in development!"
  public success: boolean = false;
  public buttonState: string = "Create";
  public updatePost: Post;
  
  constructor(
    private modalService: NgbModal,
    private postsService: PostsService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.getPosts();
    this.postForm = this.formBuilder.group({
      Caption: ['', [Validators.required]],
      ImageUrl: [''],
    });
  }


  getPosts() {
      this.postsService.getPosts().subscribe(
        (res) => {
          this.posts = res.posts;
          this.updateLoading(false);
        },
        (err) => {
          console.log(err);
        }
      );
  }

  doPost() {
    if (this.buttonState === "Create") {
      this.postsService.postPost(this.postForm.value).subscribe(
        (res) => {
          this.errorMessage = "";
          this.success = true;
          this.getPosts();
        },
        (err) => {
          this.success = false;
          console.log(err);
          if (err.error && err.error.errors) {
            if (err.error.errors.Caption) {
              this.errorMessage = err.error.errors.Caption[0];
              return;
            }
            if (err.error.errors.ImageUrl) {
              this.errorMessage = err.error.errors.ImageUrl[0];
              return;
            }
          }
        }
      )
    }
    if (this.buttonState === "Update") {
      this.updatePost.caption = this.postForm.value.Caption;
      this.updatePost.imageUrl = this.postForm.value.ImageUrl;
      this.postsService.putPost(this.updatePost).subscribe(
        (res) => {
          this.errorMessage = "";
          this.success = true;
          this.getPosts();
          this.buttonState = "Create";
        },
        (err) => {
          this.success = false;
          console.log(err);
          if (err.error && err.error.errors) {
            if (err.error.errors.Caption) {
              this.errorMessage = err.error.errors.Caption[0];
              return;
            }
            if (err.error.errors.ImageUrl) {
              this.errorMessage = err.error.errors.ImageUrl[0];
              return;
            }
          }
        }
      )
    }
  }

  doDelete(id: Number) {
    this.postsService.deletePost(id).subscribe(
      (res) => {
        this.getPosts();
      },
      (err) => {
        console.log(err);
        this.alertMessage = err;
      }
    )
  }

  doUpdate(post: Post) {
    this.updatePost = post;
    this.buttonState = "Update";
    this.postForm.setValue({Caption: post.caption, ImageUrl: post.imageUrl});
  }

  updateLoading(loading: boolean) {
    this.loadingChange.emit(loading);
  }

}
