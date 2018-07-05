using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using ZeroDaySocial.Models;

namespace ZeroDaySocial.Repositories
{
    public interface ITwitterApplicationClient
    {
        Task<string> CreateUser(Models.ZeroDayTwitterUser user);
        Task<string> CreateUser(Tweetinvi.Models.IAuthenticatedUser user);
        Task<string> CreateCredentials(TwitterUserCredentials twitterUserCredentials);
        Task<Models.ZeroDayTwitterUser> GetUser(string twitterId);
    }
}
