using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroDaySocial.Controllers
{
    [Route("[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly Repositories.ITwitterApplicationClient _twitterClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(Repositories.ITwitterApplicationClient twitterClient, IHttpContextAccessor httpContextAccessor)
        {
            _twitterClient = twitterClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var twitterId = _httpContextAccessor.HttpContext?.Request?.Cookies["twitterId"];

            if (!(string.IsNullOrEmpty(twitterId)))
                return RedirectToAction("Index", new { twitterId = twitterId });

            return View();
        }

        [HttpGet("{twitterId}")]
        public async Task<IActionResult> Index(string twitterId)
        {
            if(string.IsNullOrEmpty(twitterId))
                twitterId = _httpContextAccessor.HttpContext?.Request?.Cookies["twitterId"];

            if (twitterId == null)
                return RedirectToAction("twitterAuth", "twitterAuthentication");

            var user = await _twitterClient.GetUser(twitterId);

            if (user == null)
                return new NotFoundObjectResult(new { message = $"twitterId {twitterId} does not exist in database" });

            return View(user);
        }
    }
}
