import { Component, OnInit } from '@angular/core';
import { environment } from './../environments/environment';
import { ExceptionService } from './services/http/exception.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = environment.title;

  constructor(private exceptionService:ExceptionService){

  }
  
  ngOnInit(): void {
    this.exceptionService.businessException().subscribe();
  }
}
