import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field'
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import {MatProgressBarModule} from '@angular/material/progress-bar';

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatProgressBarModule
  ],
  providers: [],
  bootstrap: [],
  exports: [
    MatDialogModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatProgressBarModule
  ],
})
export class AngularMaterialModule { }