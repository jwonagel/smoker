import { Component, OnInit } from '@angular/core';
import { SmokerService, MeasurementSmoker } from '../services/api';
import { SignalRService } from '../services/signalr/signal-r.service';
import { MatSliderChange } from '@angular/material/slider';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { OpenCloseModel } from '../services/signalr/open-close-model.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  constructor(private smokerService: SmokerService,
              private signalRService: SignalRService,
              private snackBar: MatSnackBar) {
                this.connectionEstablished = signalRService.connectionEstablieshedValue;
               }

  values: string[];
  latestMeasurement: MeasurementSmoker;
  autoMode: boolean;
  openClose: number;
  private connectionEstablished: boolean;

  // txtMessage = '';

  ngOnInit(): void {
    this.getLatestMeasurement();
    this.signalRService.connectionEstablished
      .subscribe((e: boolean) => this.connectionEstablished = e);
    this.signalRService.measurementUpdateEvent.subscribe((value:string) => {
      if (value === 'Update'){
        this.getLatestMeasurement();
      }
    });
  }

  private getLatestMeasurement(): void {
    this.smokerService.smokerLatestGet()
      .subscribe((e: MeasurementSmoker) =>
        {
          this.latestMeasurement = e;
          this.autoMode = e.isAutoMode;
          this.openClose = e.openCloseState * 100;
        });
  }


  onAutoModeToggleChange(event: MatSlideToggleChange): void {
    this.updateOpenCloseModel();
  }

  onOpenCloseChangeEvent(event: MatSliderChange): void {
    this.updateOpenCloseModel();
  }



  private updateOpenCloseModel(): void {
    const openCloseModel = new OpenCloseModel();
    openCloseModel.isAutoMode = this.autoMode;
    openCloseModel.openCloseState = this.openClose / 100;
    if (this.connectionEstablished) {
      this.signalRService.sendUpdateCloseState(openCloseModel);
    } else {
      this.snackBar.open('Fehler', 'Keine Verbindung', {
        duration: 5000
      });
    }
  }

  // private subscribeToEvents(): void {
  //   this.signalRService.messageReceived.subscribe((message: string) => {
  //     this.ngZone.run(() => {
  //         this.messages.push(message);
  //     });
  //   });
  // }

}
