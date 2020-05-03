// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("smoker_api", "Smoker API")
                {
                    Scopes = {new Scope("smoker_api.read")}
                }
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                // new Client
                // {
                //     ClientId = "client",
                //     ClientName = "Client Credentials Client",

                //     AllowedGrantTypes = GrantTypes.ClientCredentials,
                //     ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //     AllowedScopes = { "smoker_api" }
                // },

                // // MVC client using code flow + pkce
                // new Client
                // {
                //     ClientId = "mvc",
                //     ClientName = "MVC Client",

                //     AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                //     RequirePkce = true,
                //     ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //     RedirectUris = { "http://localhost:5003/signin-oidc" },
                //     FrontChannelLogoutUri = "http://localhost:5003/signout-oidc",
                //     PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },

                //     AllowOfflineAccess = true,
                //     AllowedScopes = { "openid", "profile", "smoker_api" }
                // },

                // SPA client using code flow + pkce
                new Client
                {
                    RequireConsent = false,
                    ClientId = "spa",
                    ClientName = "Smoker SPA",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes = { "openid", "profile", "email", "smoker_api.read", "smoker_api.write" },
                    RedirectUris = {"https://localhost:4200/home"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    // AllowAccessTokensViaBrowser = true,
                    
                    // AccessTokenLifetime = 3600
                }
            };
    }
}