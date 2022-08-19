using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web.Models
{
    public class LoginResponse
    {
        [JsonProperty("Token")]
        public string Token { get; set; }
    }
}
