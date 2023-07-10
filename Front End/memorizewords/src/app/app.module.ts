import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { AddNewWordComponent } from './pages/main-page/add-new-word/add-new-word.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './modules/angular-material.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AlertifyService } from './services/alertify-service.service';
import { AnswersComponent } from './pages/main-page/answers/answers.component';
import { AgGridModule } from 'ag-grid-angular';
import { AngularGridModule } from './modules/angular-grid.module';
import { BooleanAgColumnComponent } from './core/components/ag-grid/column/boolean-ag-column/boolean-ag-column.component';
import { QuestionComponent } from './pages/main-page/question/question.component';
import { ProgressbarAgColumnComponent } from './core/components/ag-grid/column/progressbar-ag-column/progressbar-ag-column.component';
import { ErrorInterceptor } from './services/interceptor/error-interceptor';
import { TextColumnComponent } from './core/components/ag-grid/column/text-column/text-column.component';
import { HttpResponseInterceptor } from './services/interceptor/http-response-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    AddNewWordComponent,
    AnswersComponent,
    BooleanAgColumnComponent,
    QuestionComponent,
    ProgressbarAgColumnComponent,
    TextColumnComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
    HttpClientModule,
    AngularGridModule
  ],
  providers: [AlertifyService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpResponseInterceptor,
      multi: true
    },],
  bootstrap: [AppComponent]
})
export class AppModule { }
