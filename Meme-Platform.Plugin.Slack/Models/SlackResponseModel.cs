using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meme_Platform.Plugin.Slack.Models
{
    public class SlackResponseModel
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }


        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
