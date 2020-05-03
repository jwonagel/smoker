import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(public oidcSecurityService: OidcSecurityService) {}

  ngOnInit(): void {
    this.oidcSecurityService.checkAuth().subscribe((isAuthenticated) => {
      console.log('is authenticated:' + isAuthenticated);
      if (!isAuthenticated){
        this.login();
      }
    });

    const payload = this.oidcSecurityService.getToken();
    console.log(payload);
    const foo = this.oidcSecurityService.getPayloadFromIdToken();
    console.log(foo);
    const token = this.oidcSecurityService.getIdToken();
    console.log(token);
  }

  login() {
    this.oidcSecurityService.authorize();
  }
}
