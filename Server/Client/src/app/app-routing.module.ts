import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OverviewComponent } from './overview/overview.component';
import { SettingsComponent } from './settings/settings.component';
import { SigninOidcComponent } from './signin-oidc/signin-oidc.component';
import { RequireAuthenticateduserRouteGuardService } from './services/require-authenticateduser-route-guard.service';
import { RedirectSilentRenewComponent } from './redirect-silent-renew/redirect-silent-renew.component';


const routes: Routes = [
  {path: 'home', component: OverviewComponent,
   canActivate: [RequireAuthenticateduserRouteGuardService]},
  {path: 'settings', component: SettingsComponent,
   canActivate: [RequireAuthenticateduserRouteGuardService]},
  {path: '', redirectTo: '/home', pathMatch: 'full',
    canActivate: [RequireAuthenticateduserRouteGuardService]},
  {path: 'signin-oidc', component: SigninOidcComponent},
  {path: 'redirect-silent-renew', component: RedirectSilentRenewComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
