import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './footer/footer.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSliderModule } from '@angular/material/slider';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { MainMenuComponent } from './main-menu/main-menu.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { OverviewComponent } from './overview/overview.component';
import { SettingsComponent } from './settings/settings.component';
import { ApiModule, BASE_PATH } from './services/api';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { TemperaturPipe } from './pipes/temperatur.pipe';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faThermometerHalf, faFireAlt, faBacon, faClock } from '@fortawesome/free-solid-svg-icons';
import { SensorItemComponent } from './overview/sensor-item/sensor-item.component';
import { OpenIdConnectService } from './services/auth/open-id-connect.service';
import { SigninOidcComponent } from './signin-oidc/signin-oidc.component';
import { RequireAuthenticateduserRouteGuardService } from './services/require-authenticateduser-route-guard.service';
import { AddAuthorizationHeaderInterceptor } from './services/add-authorization-header-interceptor';
import { RedirectSilentRenewComponent } from './redirect-silent-renew/redirect-silent-renew.component';
import { from } from 'rxjs';
import {  FormsModule } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';


@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    MainMenuComponent,
    OverviewComponent,
    SettingsComponent,
    TemperaturPipe,
    SensorItemComponent,
    SigninOidcComponent,
    RedirectSilentRenewComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    MatSliderModule,
    MatSlideToggleModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatFormFieldModule,
    MatInputModule,
    ApiModule,
    HttpClientModule,
    FontAwesomeModule,
    FormsModule,
    MatGridListModule,
    MatCardModule,
    MatSnackBarModule
  ],
  providers: [
    {provide: BASE_PATH, useValue: environment.API_BASE_PATH},
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AddAuthorizationHeaderInterceptor,
      multi: true
    },
    OpenIdConnectService,
    RequireAuthenticateduserRouteGuardService
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
