import { Component, OnInit } from '@angular/core';
import { environment } from '../environments/environment';
import { SignalRService } from './core/services/hub/signalr/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor() {
    console.info("environment: " + environment.environmentName);
  }

}

