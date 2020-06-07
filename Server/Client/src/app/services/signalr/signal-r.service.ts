import { Injectable, EventEmitter } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { OpenIdConnectService } from '../auth/open-id-connect.service';
import { OpenCloseModel } from './open-close-model.model';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  // messageReceived = new EventEmitter<string>();
  connectionEstablished = new EventEmitter<boolean>();
  settingsUpdateEvent = new EventEmitter<string>();
  measurementUpdateEvent = new EventEmitter<string>();
  connectionEstablieshedValue = false;

  private hubConnection: HubConnection;

  constructor(private openIdConnectService: OpenIdConnectService) {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  sendMessage(message: string) {
    this.hubConnection.invoke('SendMessage','test', message);
  }

  sendUpdateCloseState(openCloseModel: OpenCloseModel): void {
    this.hubConnection.invoke('SendUpdateCloseState', openCloseModel);
  }

  private createConnection() {
    const url = environment.API_BASE_PATH + '/messagehub';
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(url, { accessTokenFactory: () => this.openIdConnectService.user.access_token })
      .configureLogging(LogLevel.Debug)
      .build();
  }


  private startConnection(): void {
    const that = this;
    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
        this.connectionEstablieshedValue = true;
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        console.log(err);
        setTimeout(() => { that.startConnection(); }, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('ReceiveMessage', (type: string, content: string) => {
      switch (type) {
        case 'Settings':
          this.settingsUpdateEvent.emit(content);
          break;
        case 'Measurement':
          this.measurementUpdateEvent.emit(content);
          break;
        default:
          console.warn(`Event ${type} with Content ${content} not handled`);
          break;
      }
    });
  }
}
