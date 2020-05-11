import { Component, OnInit } from '@angular/core';
import { OpenIdConnectService } from './services/auth/open-id-connect.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private openIdConnectService: OpenIdConnectService,
              private router: Router) {}

  ngOnInit(): void {
    this.openIdConnectService.userLoaded$.subscribe(userLoaded => {
      if (userLoaded) {
        this.router.navigate(['./']);
      } else {
        if (!environment.production) {
          console.log('An error happened: user was not laoded');
        }
      }
    });
  }
}
