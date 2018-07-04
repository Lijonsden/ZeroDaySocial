using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroDaySocial.Models
{

    public class ZeroDayTwitterUser
    {
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string TwitterId { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}
