// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace AuthServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "Your role", new []{"role"})
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("smokerapi", "Smmoker API", new [] {"role", "given_name", "family_name"})
            };


        public static IEnumerable<Client> GetClients()
        {
            var clientUri = Environment.GetEnvironmentVariable("ClientUrl");
            var smokerSecret = Environment.GetEnvironmentVariable("smokerSecret");
            return new Client[]
            {
                // SPA client using code flow + pkce
                new Client
                {
                    ClientName = "Smoker Client",
                    ClientId = "smokerclient",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string> {
                        $"{clientUri}/signin-oidc",
                        $"{clientUri}/redirect-silent-renew"
                    },
                    AccessTokenLifetime = 180,
                    PostLogoutRedirectUris = new List<string> {
                        $"{clientUri}/"
                    },
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "smokerapi"
                    }
                },
                new Client()
                {
                    ClientName = "Smoker",
                    ClientId = "smoker",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = 
                    {
                        new Secret(smokerSecret.Sha256())
                    },
                    AllowedScopes = {
                        "smokerapi"
                    }                   
                }
            };
        }
    }
}