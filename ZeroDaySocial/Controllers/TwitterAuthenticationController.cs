using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace ZeroDaySocial.Controllers
{
    public class TwitterAuthenticationController : Controller
    {
        private readonly Repositories.ITwitterApplicationClient _twitterApplication;
        private IAuthenticationContext _authenticationContext;
        private IHttpContextAccessor _httpContextAccessor;
        private Options.TwitterApiConfiguration _options;
        private readonly ILogger _logger;

        public TwitterAuthenticationController(IOptions<Options.TwitterApiConfiguration> options, 
            IHttpContextAccessor accessor, ILogger<TwitterAuthenticationController> logger, 
            Repositories.ITwitterApplicationClient twitterApplication)
        {
            _options = options.Value;
            _httpContextAccessor = accessor;
            _logger = logger;
            _twitterApplication = twitterApplication;
        }

        [HttpGet]
        public async Task<IActionResult> TwitterAuth()
        {
            var callbackUrl = "http://localhost:61624/twitterauthentication/ValidateTwitterAuth";
            _authenticationContext = AuthFlow.InitAuthentication(new ConsumerCredentials(_options.ConsumerKey, 
                _options.ConsumerSecret), 
                callbackUrl);

            if (_authenticationContext == null)
                throw new Exception("no authentication context");

            return new RedirectResult(_authenticationContext.AuthorizationURL);
        }

        [HttpGet]
        public async Task<IActionResult> ValidateTwitterAuth(string oauth_verifier, string authorization_id)
        {
            if (string.IsNullOrEmpty(oauth_verifier?.ToString()))
                return StatusCode(401);

            var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(oauth_verifier, authorization_id);

            if (userCreds == null)
                return StatusCode(401);

            var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);

            if (user == null)
                return StatusCode(401);

            var twitterId = await _twitterApplication.CreateUser(user);
            await _twitterApplication.CreateCredentials(new Models.TwitterUserCredentials()
            {
                AccessToken = userCreds.AccessToken,
                AccessTokenSecret = userCreds.AccessTokenSecret,
                TwitterId = twitterId,
                ValidUntil = DateTime.UtcNow
            }); 

            _httpContextAccessor.HttpContext.Response.Cookies.Append("twitterId", twitterId, new CookieOptions() { Secure = false, Expires = DateTime.UtcNow.AddSeconds(20) }); 

            return new RedirectToActionResult("Index", "Dashboard", new { twitterId = twitterId });
        }
    }
}
