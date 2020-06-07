import { Component, OnInit } from '@angular/core';
import { ClientSettingsService, SettingsClient,  } from '../services/api';
import {MatSnackBar} from '@angular/material/snack-bar';
import { SignalRService } from '../services/signalr/signal-r.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  settings: SettingsClient;
  fireNotificationActiveVisible: boolean;

  private isSaving = false;

  constructor(private settingsService: ClientSettingsService,
              private snackBar: MatSnackBar,
              private signalRService: SignalRService) { }


  ngOnInit(): void {
    this.loadSettings();
    this.signalRService.settingsUpdateEvent.subscribe(message => {
      if (message === 'Update') {
        if (!this.isSaving) {
          this.loadSettings();
          this.snackBar.open('Benachrichtigung', 'Einstelungen aktualisiert', {
            duration: 3000
          });
        }
      }
    });
  }

  onSubmit() {
    this.isSaving = true;
    this.settingsService.clientSettingsCurrentPost(this.settings)
      .subscribe((s: SettingsClient) => {
        this.settings = s;
        this.snackBar.open('Benachrichtigung', 'Einstellungen gespeichert', {
          duration: 3000
        });
        this.isSaving = false;
      });
  }

  isFireNotificationTemp(): boolean {
    return !this.settings?.fireNotifcationTemperatur == null;
  }

  fireNotifactionActiveToggleChanged(event: any): void {
    this.fireNotificationActiveVisible = event.checked;
    if (event.checked) {
      this.settings.fireNotifcationTemperatur = 80;
    } else {
      this.settings.fireNotifcationTemperatur = null;
    }
  }

  private loadSettings(): void
  {
    this.settingsService.clientSettingsCurrentGet()
      .subscribe(s => {
        this.settings = s;
        this.fireNotificationActiveVisible = this.settings?.fireNotifcationTemperatur !== null;

      });
  }

}
