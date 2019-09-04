import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  
  values:any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getBooks();
  }

  getBooks(){
    
    this.http.get('http://localhost:5000/api/Books').subscribe(
      response=>{this.values = response},
      error=>{console.log(error)}
    );
  }

}
