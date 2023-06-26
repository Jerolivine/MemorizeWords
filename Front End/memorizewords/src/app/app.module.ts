import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { AddNewWordComponent } from './pages/main-page/add-new-word/add-new-word.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './modules/angular-material.module';
import { HttpClientModule } from '@angular/common/http';
import { AlertifyService } from './services/alertify-service.service';
import { AnswersComponent } from './pages/main-page/answers/answers.component';
import { AgGridModule } from 'ag-grid-angular';
import { AngularGridModule } from './modules/angular-grid.module';
import { BooleanAgColumnComponent } from './core/components/ag-grid/column/boolean-ag-column/boolean-ag-column.component';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    AddNewWordComponent,
    AnswersComponent,
    BooleanAgColumnComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
    HttpClientModule,
    AngularGridModule
  ],
  providers: [AlertifyService],
  bootstrap: [AppComponent]
})
export class AppModule { }
