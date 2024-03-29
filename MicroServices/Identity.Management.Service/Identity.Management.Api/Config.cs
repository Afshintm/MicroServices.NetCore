﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.Management.Api
{
    public class Config
    {
        
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
                ,new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    Description = "Allow the service access to your user roles.",
                    UserClaims = new[] { JwtClaimTypes.Role, ClaimTypes.Role },
                    ShowInDiscoveryDocument = true,
                    Required = true,
                    //Emphasize = true
                }

            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
                ,new ApiResource("Inventory.Service.Api", "Essence Communication Api")
                
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1"
                    , "Inventory.Service.Api"
                    }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1"
                    , "Inventory.Service.Api"
                    }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = true,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1","Inventory.Service.Api","roles"
                    },
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                },
                new Client
                    {
                        ClientName = "angularclient",
                        ClientId = "angularclient",
                        AccessTokenType = AccessTokenType.Reference,
                        //AccessTokenLifetime = 600, // 10 minutes, default 60 minutes
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowAccessTokensViaBrowser = true,
                        RedirectUris = new List<string>
                        {
                            "http://localhost:4200/auth-callback"

                        },
                        PostLogoutRedirectUris = new List<string>
                        {
                            "http://localhost:4200/Unauthorized"
                        },
                        AllowedCorsOrigins = new List<string>
                        {
                            "http://localhost:4200"
                        },
                        AllowedScopes = new List<string>
                        {

                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "api1","Inventory.Service.Api","roles"
                        }
                    },
                new Client
                    {
                        ClientName = "angularclient-aws",
                        ClientId = "angularclient-aws",
                        AccessTokenType = AccessTokenType.Reference,
                        //AccessTokenLifetime = 600, // 10 minutes, default 60 minutes
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowAccessTokensViaBrowser = true,
                        RedirectUris = new List<string>
                        {
                            "http://app.homestay.care/auth-callback"

                        },
                        PostLogoutRedirectUris = new List<string>
                        {
                            "http://app.homestay.care/Unauthorized"
                        },
                        AllowedCorsOrigins = new List<string>
                        {
                            "http://app.homestay.care"
                        },
                        AllowedScopes = new List<string>
                        {

                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "api1","Inventory.Service.Api","roles"
                        }
                    }



            };
        }

        //public static List<TestUser> GetUsers()
        //{
        //    return new List<TestUser>
        //    {
        //        new TestUser
        //        {
        //            SubjectId = "1",
        //            Username = "alice",
        //            Password = "password",

        //            Claims = new List<Claim>
        //            {
        //                new Claim("name", "Alice"),
        //                new Claim("website", "https://alice.com")
        //            }
        //        },
        //        new TestUser
        //        {
        //            SubjectId = "2",
        //            Username = "bob",
        //            Password = "password",

        //            Claims = new List<Claim>
        //            {
        //                new Claim("name", "Bob"),
        //                new Claim("website", "https://bob.com")
        //            }
        //        }
        //    };
        //}

    }
}
