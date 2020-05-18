import { Injectable } from '@angular/core';
import { UserManager, User } from "oidc-client";
import { environment } from 'src/environments/environment';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OpenIdConnectService {

  private userManager: UserManager;
  private currentUser: User;

  userLoaded$ = new ReplaySubject<boolean>(1);

  get userAvailable(): boolean {
    return this.currentUser != null;
  }

  get user(): User {
    return this.currentUser;
  }

  constructor() {
    this.userManager = new UserManager({
      authority: environment.stsServer,
      client_id: 'smokerclient',
      redirect_uri: `${window.location.origin}/signin-oidc`,
      scope: 'openid profile roles smokerapi',
      response_type: 'id_token token',
      post_logout_redirect_uri: `${window.location.origin}/`,
      automaticSilentRenew: true,
      silent_redirect_uri: `${window.location.origin}/redirect-silentrenew`
    });
    this.userManager.clearStaleState();

    this.userManager.events.addUserLoaded(u => {
      if (!environment.production) {
        console.log('User loaded', u);
      }
      this.currentUser = u;
      this.userLoaded$.next(true);
    });

    this.userManager.events.addUserUnloaded(() => {
      if (!environment.production) {
        console.log('User unloaded');
      }
      this.currentUser = null;
      this.userLoaded$.next(false);
    });

  }


  triggerSignIn() {
    this.userManager.signinRedirect().then(() => {
      if (!environment.production){
        console.log('Signin triggerted');
      }
    });
  }

  handleCallback() {
    this.userManager.signinRedirectCallback().then(u => {
      if (!environment.production){
        console.log('Callback after logon', u);
      }
    });
  }

  handleSilentCallback() {
    this.userManager.signinSilentCallback().then((user) => {
      this.currentUser = user;
      if (!environment.production){
        console.log('Callback after silent signin handled', user);
      }
    });
  }

  triggerSignOut() {
    this.userManager.signoutRedirect().then(resp => {
      if (!environment.production) {
        console.log('Redirection to sign out triggerted');
      }
    });
  }
}
