import { HubConnectionBuilder, HubConnection, IHttpConnectionOptions } from '@microsoft/signalr';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as signalR from '@microsoft/signalr';
import {  Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class SignalRService {

  private eventDictionary = new Map<string, Subject<any>>();
  private hubsDictionary = new Map<string, HubConnection>();

  constructor() {

  }

  startEvent(hubName: string, eventName: string): Observable<any> {
    const eventSubject = new Subject<any>();
    this.eventDictionary.set(eventName, eventSubject);

    var hubConnection = this.createHubConnection(hubName);
    hubConnection.start().then(() => {
      // console.log('SignalR connection started.');
    })
      .catch(err => {
        // console.error('Error starting SignalR connection:', err);
      });

    hubConnection.on(eventName, (data) => {
      var obj = JSON.parse(data);
      eventSubject.next(obj);
      // console.log('Event received:', data);
    });

    this.hubsDictionary.set(eventName, hubConnection);
    return eventSubject.asObservable();
  }

  stopEvent(eventName: string) {
    this.hubsDictionary.get(eventName)?.stop();
    this.hubsDictionary.delete(eventName);

    this.eventDictionary.get(eventName)?.complete();
    this.eventDictionary.delete(eventName);
  }

  private createHubConnection(hubName: string): HubConnection {

    return new HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/${hubName}`,
        {
          skipNegotiation: true,
          withCredentials: true,
          transport: signalR.HttpTransportType.WebSockets,
          accessTokenFactory() {
            // TODO-Arda : Token Handler
            return "0";
          }
        })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Error)
      .build();

  }

}
