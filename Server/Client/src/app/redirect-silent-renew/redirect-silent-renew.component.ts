import { Component, OnInit } from '@angular/core';
import { OpenIdConnectService } from '../services/auth/open-id-connect.service';

@Component({
  selector: 'app-redirect-silent-renew',
  templateUrl: './redirect-silent-renew.component.html',
  styleUrls: ['./redirect-silent-renew.component.scss']
})
export class RedirectSilentRenewComponent implements OnInit {

  constructor(private openIdConnectService: OpenIdConnectService) { }

  ngOnInit(): void {
    this.openIdConnectService.handleSilentCallback();
  }

}
