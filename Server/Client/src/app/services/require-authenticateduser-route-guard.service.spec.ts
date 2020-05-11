import { TestBed } from '@angular/core/testing';

import { RequireAuthenticateduserRouteGuardService } from './require-authenticateduser-route-guard.service';

describe('RequireAuthenticateduserRouteGuardService', () => {
  let service: RequireAuthenticateduserRouteGuardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RequireAuthenticateduserRouteGuardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
