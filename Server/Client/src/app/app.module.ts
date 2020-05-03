import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './footer/footer.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSliderModule } from '@angular/material/slider';
import { MainMenuComponent } from './main-menu/main-menu.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { OverviewComponent } from './overview/overview.component';
import { SettingsComponent } from './settings/settings.component';
import { ApiModule, BASE_PATH } from './services/api';
import { HttpClientModule } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { TemperaturPipe } from './pipes/temperatur.pipe';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faThermometerHalf, faFireAlt, faBacon, faClock } from '@fortawesome/free-solid-svg-icons';
import { SensorItemComponent } from './overview/sensor-item/sensor-item.component';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    MainMenuComponent,
    OverviewComponent,
    SettingsComponent,
    TemperaturPipe,
    SensorItemComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    MatSliderModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    ApiModule,
    HttpClientModule,
    FontAwesomeModule
  ],
  providers: [
    {provide: BASE_PATH, useValue: environment.API_BASE_PATH}
  ],
  bootstrap: [
    AppComponent,
  ]
})
export class AppModule {
  constructor(library: FaIconLibrary){
    library.addIcons(faThermometerHalf, faFireAlt, faBacon, faClock);
  }
 }
