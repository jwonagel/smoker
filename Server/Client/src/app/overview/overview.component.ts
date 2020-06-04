import { Component, OnInit, NgZone } from '@angular/core';
import { SmokerService, MeasurementClient } from '../services/api';
import { SignalRService } from '../services/signalr/signal-r.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  constructor(private smokerService: SmokerService,
              private signalRService: SignalRService,
              private ngZone: NgZone) { }

  values: string[];
  latestMeasurement: MeasurementClient;
  messages = new Array<string>();
  // txtMessage = '';

  ngOnInit(): void {
    this.smokerService.smokerLatestGet()
      .subscribe(e => this.latestMeasurement = e);

    this.subscribeToEvents();
  }

  sendMessage(message: string){
    if (message){
      this.signalRService.sendMessage(message);
    }
  }

  private subscribeToEvents(): void {
    this.signalRService.messageReceived.subscribe((message: string) => {
      this.ngZone.run(() => {
          this.messages.push(message);
      });
    });
  }

}
