import { Injectable, EventEmitter } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { OpenIdConnectService } from '../auth/open-id-connect.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  messageReceived = new EventEmitter<string>();
  connectionEstablished = new EventEmitter<boolean>();

  private hubConnection: HubConnection;

  constructor(private openIdConnectService: OpenIdConnectService) {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  sendMessage(message: string) {
    this.hubConnection.invoke('SendMessage','test', message);
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
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        console.log(err);
        setTimeout(() => { that.startConnection(); }, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('ReceiveMessage', (type: string, content: string) => {
      this.messageReceived.emit(content);
    });
  }
}
