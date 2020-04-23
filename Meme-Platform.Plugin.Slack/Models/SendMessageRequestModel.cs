using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meme_Platform.Plugin.Slack.Models
{
    internal class SendMessageRequestModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
