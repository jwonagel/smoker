import { Component, OnInit } from '@angular/core';
import { OpenIdConnectService } from '../services/auth/open-id-connect.service';
import { Router } from '@angular/router';
import { faUserLock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-signin-oidc',
  templateUrl: './signin-oidc.component.html',
  styleUrls: ['./signin-oidc.component.scss']
})
export class SigninOidcComponent implements OnInit {

  constructor(private openIdConnectService: OpenIdConnectService,
              private router: Router) { }

  ngOnInit(): void {
    this.openIdConnectService.userLoaded$.subscribe((userLoaded) => {
      if (userLoaded) {
        this.router.navigate(['./']);
      }
    });
    this.openIdConnectService.handleCallback();
  }

}
