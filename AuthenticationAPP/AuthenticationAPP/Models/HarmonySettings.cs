using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationAPP.Models
{
    public class HarmonySettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApiUsername { get; set; }
        public string ApiPassword { get; set; }
        public string AuthUrl { get; set; }
        public Content content { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string parentId { get; set; }
        public string type { get; set; }
        public string subType { get; set; }
        public string OUID { get; set; }

    }
}