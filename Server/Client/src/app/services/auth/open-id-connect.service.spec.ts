import { TestBed } from '@angular/core/testing';

import { OpenIdConnectService } from './open-id-connect.service';

describe('OpenIdConnectService', () => {
  let service: OpenIdConnectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OpenIdConnectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
