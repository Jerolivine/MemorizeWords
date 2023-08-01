import { Component, OnInit } from '@angular/core';
import { environment } from '../environments/environment';
import { SignalRService } from './core/services/hub/signalr/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor() {
    console.info("environment: " + environment.environmentName);
    console.info("version" + environment.version);
  }

  ngOnInit(): void {

    // const userGuessedWordsHubObservable = this.signalRService.startEvent("userGuessedWordsHub", "ReceiveUserGuessedWords");
    // userGuessedWordsHubObservable.subscribe(data =>{
    //   debugger;
    //   console.log("Next Data :" + JSON.stringify(data));
    // });

    // setTimeout(() => {
    //   this.signalRService.stopEvent("ReceiveUserGuessedWords");
    //   console.log("event stopped");
    // },5000)

  }

}

