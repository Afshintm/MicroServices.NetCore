//using Essence.Communication.DbContexts;
//using Essence.Communication.Models;
//using Essence.Communication.Models.IdentityModels;
using Identity.Management.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Management.Api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager) {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException($"user with sub{sub} not found.");
            }

            //We may need to manually add roleClaims to the claims
            // at the end of this method like context.IssuedClaims.AddRange(roleClaims);
            //var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);
            //List<string> list = context.RequestedClaimTypes.ToList();

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            //Add more claims like this
            //claims.Add(new System.Security.Claims.Claim("MyProfileID", user.Id));

            //this will filter claims based on requested claims
            //context.AddRequestedClaims(claims);

            // we are not overwriting 
            //context.IssuedClaims = claims;
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
