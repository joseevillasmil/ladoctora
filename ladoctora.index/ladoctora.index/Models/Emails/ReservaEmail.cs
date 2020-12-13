using System;
using Newtonsoft.Json;

namespace ladoctora.index.Models.Emails
{
    public class ReservaEmail
    {
        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("date")]
        public string date { get; set; }

    }
}
