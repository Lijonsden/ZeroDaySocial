using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroDaySocial.Models.ConverterModels
{
    public class ResponseTwitterUser
    {
        public class JsonDeserialize
        {
            [JsonProperty("result")]
            public ZeroDayTwitterUser result { get; set; }
        }

        public class ZeroDayTwitterUser
        {
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("ScreenName")]
            public string ScreenName { get; set; }
            [JsonProperty("TwitterId")]
            public string TwitterId { get; set; }
            [JsonProperty("AccessToken")]
            public string AccessToken { get; set; }
            [JsonProperty("AccessTokenSecret")]
            public string AccessTokenSecret { get; set; }
        }
    }
}
