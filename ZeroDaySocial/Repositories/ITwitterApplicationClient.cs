using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroDaySocial.Repositories
{
    public interface ITwitterApplicationClient
    {
        Task<string> CreateUser(Models.ZeroDayTwitterUser user);
        Task<string> CreateUser(Tweetinvi.Models.IAuthenticatedUser user);
        Task<Models.ZeroDayTwitterUser> GetUser(string twitterId);
    }
}
