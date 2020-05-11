import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { OpenIdConnectService } from './auth/open-id-connect.service';


@Injectable({
  providedIn: 'root'
})
export class RequireAuthenticateduserRouteGuardService implements CanActivate {

  constructor(private router: Router,
              private openIdConnectService: OpenIdConnectService) { }


  canActivate(): boolean {
    if (this.openIdConnectService.userAvailable) {
      return true;
    } else {
      this.openIdConnectService.triggerSignIn();
      return false;
    }
  }
}
