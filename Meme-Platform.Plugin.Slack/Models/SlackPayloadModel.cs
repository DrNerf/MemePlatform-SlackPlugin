using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meme_Platform.Plugin.Slack.Models
{
    internal class SlackPayloadModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("icon_url")]
        public string Icon { get; set; }

        [JsonProperty("attachments")]
        public IList<AttachmentModel> Attachments { get; set; } = new List<AttachmentModel>();
    }
}
