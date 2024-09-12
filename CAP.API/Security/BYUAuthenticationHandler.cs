// //// Add a reference to your models folder
//
// #nullable enable
// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Security.Claims;
// using System.Text.Encodings.Web;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using Microsoft.IdentityModel.Protocols;
// using Microsoft.IdentityModel.Protocols.OpenIdConnect;
// using Microsoft.IdentityModel.Tokens;
// using CAP.API.Exceptions;
// using CAP.API.Models;
// using Newtonsoft.Json;
//
// namespace CAP.API.Security
// {
//     public static class AuthenticationSchemeConstants
//     {
//         public const string BYUAuthenticationName = "BYUAuthentication";
//     }
//
//     public class BYUAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//     {
//         private ICollection<SecurityKey>? _securityKeys;
//
//         private Timer? _cacheTimer;
//
//         private const string WellKnownUrl = "https://api.byu.edu/.well-known/openid-configuration";
//
//         private readonly IConfiguration _config;
//
//         // Replace this line with your context, and replace the object type below
//         private readonly DbContext _context;
//
//         public BYUAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
//             UrlEncoder encoder, ISystemClock clock, DbContext context, IConfiguration config)
//             : base(options, logger, encoder, clock)
//         {
//             _context = context;
//             _config = config;
//         }
//
//         protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//         {
//             try
//             {
//                 var jwt = Request.Headers["x-jwt-assertion"].SingleOrDefault();
//                 if (jwt.IsNullOrEmpty())
//                 {
//                     jwt = await GetJwtFromEchoService();
//                 }
//
//                 if (jwt.IsNullOrEmpty())
//                 {
//                     return AuthenticateResult.Fail("No JWT Provided");
//                 }
//
//                 var tokenHandler = new JwtSecurityTokenHandler();
//
//                 // create new validationParameters
//                 var validationParameters = new TokenValidationParameters
//                 {
//                     ValidateIssuerSigningKey = true,
//                     IssuerSigningKeys = await GetSigningKeysFromMetadataDocument(),
//                     ValidateIssuer = true,
//                     ValidIssuer = "https://api.byu.edu",
//                     ValidateAudience = false,
//                     ValidateLifetime = true,
//                     ClockSkew = TimeSpan.FromMinutes(5)
//                 };
//
//                 var tokenResult = await tokenHandler.ValidateTokenAsync(jwt, validationParameters);
//                 if (!tokenResult.IsValid)
//                 {
//                     return AuthenticateResult.Fail("Invalid JWT");
//                 }
//
//
//                 var claims = tokenResult.Claims;
//
//                 claims.TryGetValue("http://byu.edu/claims/resourceowner_net_id", out var ownerClaim);
//
//                 // Check if ownerClaim is a string and not null. 
//                 // If it is null, then we are using the client credentials grant type and we need to use the client's netid
//
//                 claims.TryGetValue("http://wso2.org/claims/applicationname", out var appNameClaim);
//
//                 if (!ValidateApplicationId(appNameClaim))
//                 {
//                     return AuthenticateResult.Fail("Application Not Permitted");
//                 }
//
//                 var client = tokenResult.ClaimsIdentity;
//
//
//                 if (ownerClaim is null)
//                 {
//                     // We are a client credential grant type
//
//                     var newClaims = new List<Claim>()
//                     {
//                         new(ClaimTypes.Name, "client"),
//                         new(ClaimTypes.GivenName, "Authorized"),
//                         new(ClaimTypes.Surname, "Application"),
//                         new(ClaimTypes.AuthenticationMethod, "client_credentials"),
//                     };
//                     client.AddClaims(newClaims);
//                 }
//                 else
//                 {
//                     // Convert ownerClaim to a string
//                     var netId = ownerClaim.ToString();
//
//                     claims.TryGetValue("http://byu.edu/claims/resourceowner_surname", out var surname);
//                     claims.TryGetValue("http://byu.edu/claims/resourceowner_preferred_first_name", out var firstName);
//
//                     if (firstName is null)
//                     {
//                         claims.TryGetValue("http://byu.edu/claims/resourceowner_rest_of_name", out firstName);
//                     }
//
//
//                     var newClaims = new List<Claim>()
//                     {
//                         new(ClaimTypes.Name, netId ?? throw new InvalidOperationException("netId is null")),
//                         new(ClaimTypes.GivenName,
//                             firstName?.ToString() ?? throw new InvalidOperationException("firstName is null")),
//                         new(ClaimTypes.Surname,
//                             surname?.ToString() ?? throw new InvalidOperationException("surname is null")),
//                         new(ClaimTypes.Role, "Unauthorized User"),
//
//                         new(ClaimTypes.AuthenticationMethod, "byu_jwt"),
//                     };
//
//                     client.AddClaims(newClaims);
//
//
//                     // Handle DB roles here
//                     // var dbUser = await _context.Users.AsNoTracking().Include(e => e.Role)
//                     //     .FirstOrDefaultAsync(u => u.NetId == netId);
//                     // if (dbUser is not null)
//                     // {
//                     //     client.AddClaim(new Claim(ClaimTypes.Role, dbUser.Role.Name));
//                     // }
//                     //
//                     // var dbStudent = await _context.Students.AsNoTracking().FirstOrDefaultAsync(u => u.NetId == netId);
//                     // if (dbStudent is not null)
//                     // {
//                     //     client.AddClaim(new Claim(ClaimTypes.Role, "Student"));
//                     // }
//
//
//                     client.AddClaim(new Claim(ClaimTypes.Role, "Unauthorized User"));
//                 }
//
//
//                 var ticket = new AuthenticationTicket(new ClaimsPrincipal(tokenResult.ClaimsIdentity), Scheme.Name);
//
//                 return AuthenticateResult.Success(ticket);
//             }
//             catch (Exception ex)
//             {
//                 Console.Error.WriteLine(ex);
//                 return AuthenticateResult.Fail("Invalid Authentication");
//             }
//         }
//
//         private async Task<string> GetJwtFromEchoService()
//         {
//             using var client = new HttpClient();
//             // just pass along the authorization header we got. If it's authenticated we're good to upload
//             client.DefaultRequestHeaders.Add("Authorization", Request.Headers["Authorization"].ToString());
//             var response = await client.GetAsync("https://api.byu.edu/echo/v2/echo");
//             if (response.StatusCode != HttpStatusCode.OK)
//                 throw new InvalidJwtException();
//
//             // Read the response as a object and get the jwt from it
//             // This is really a shim, we should move back to BYU's proxy to get the JWT
//
//             var responseString = await response.Content.ReadAsStringAsync();
//             var data = JsonConvert.DeserializeObject<dynamic>(responseString);
//             var jwt = data?.headers?["X-Jwt-Assertion"]?[0]?.ToString() ?? throw new InvalidJwtException();
//
//             return jwt;
//         }
//
//
//         /// <summary>
//         ///   Get the signing keys from the metadata document
//         /// </summary>
//         /// <returns>
//         ///  The signing keys
//         /// </returns>
//         private async Task<ICollection<SecurityKey>> GetSigningKeysFromMetadataDocument()
//         {
//             if (_securityKeys != null)
//             {
//                 return _securityKeys;
//             }
//
//             var configManager =
//                 new ConfigurationManager<OpenIdConnectConfiguration>(WellKnownUrl,
//                     new OpenIdConnectConfigurationRetriever());
//             var config = await configManager.GetConfigurationAsync();
//             var signingKeys = config.SigningKeys;
//
//             // Check how long we are meant to cache the signing keys
//             var refreshInterval = configManager.RefreshInterval;
//
//             _cacheTimer = new Timer(_ =>
//             {
//                 _securityKeys = null;
//                 _cacheTimer?.Dispose();
//             }, null, refreshInterval, Timeout.InfiniteTimeSpan);
//
//
//             _securityKeys = signingKeys;
//             return signingKeys;
//         }
//
//         /// <summary>
//         ///  Validates that the application id is a string, and that it is listed in the authorized applications
//         /// </summary>
//         /// <param name="applicationId">
//         /// The application id to validate
//         /// </param>
//         /// <returns>
//         /// True if the application id is valid, false otherwise
//         /// </returns>
//         private bool ValidateApplicationId(object? applicationId)
//         {
//             // Verify applicationId is a string
//
//             if (applicationId is not string)
//             {
//                 return false;
//             }
//
//
//             // Need to get AppSettings:AuthorizedApplications from the config
//             var authorizedApplications = _config.GetSection("AppSettings:AuthorizedApplications").Get<string>()?
//                 .Split(",")
//                 .Select(x => x.Trim());
//             // They are stored in a comma separated list
//             // Split the string into a list of strings
//             return authorizedApplications?.Contains(applicationId) ?? false;
//         }
//     }
// }