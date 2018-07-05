using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Models;
using ZeroDaySocial.Models;

namespace ZeroDaySocial.Repositories
{
    public class TwitterApplicationClient : ITwitterApplicationClient
    {
        private static HttpClient httpClient = new HttpClient();

        public async Task<string> CreateCredentials(TwitterUserCredentials twitterUserCredentials)
        {
            var url = "http://localhost:55735/api/accounts/credentials";
            var userCredentials = JsonConvert.SerializeObject(twitterUserCredentials);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, userCredentials);

            if (response.IsSuccessStatusCode)
                return twitterUserCredentials.TwitterId;
            else
                return null;
        }

        public Task<string> CreateUser(IAuthenticatedUser user)
        {
            var applicationUser = new Models.ZeroDayTwitterUser
            { Name = user.Name, ScreenName = user.ScreenName, TwitterId = user.IdStr };

            return CreateUser(applicationUser);
        }

        public async Task<string> CreateUser(Models.ZeroDayTwitterUser twitterUser)
        {
            var url = "http://localhost:55735/api/accounts/users";
            var user = JsonConvert.SerializeObject(twitterUser);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, user);

            if (response.IsSuccessStatusCode)
                return twitterUser.TwitterId;
            else 
                return null;
        }

        public async Task<ZeroDayTwitterUser> GetUser(string twitterId)
        {
            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:55735/api/accounts/users/" + twitterId);
            var responseBody = JsonConvert.DeserializeObject<Models.ConverterModels.ResponseTwitterUser.JsonDeserialize>(await response.Content.ReadAsStringAsync());

            if (responseBody?.result == null)
                return null;

            return new Models.ZeroDayTwitterUser()
            {
                Name = responseBody.result.Name,
                ScreenName = responseBody.result.ScreenName,
                TwitterId = responseBody.result.TwitterId,
            };
        }
    }
}
