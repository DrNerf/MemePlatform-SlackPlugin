using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meme_Platform.Plugin.Slack.Models
{
    internal class AttachmentModel
    {
        [JsonProperty("fallback")]
        public string Fallback { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("pretext")]
        public string PreText { get; set; }

        [JsonProperty("footer")]
        public string Footer { get; set; }

        [JsonProperty("footer_icon")]
        public string FooterIcon { get; set; }

        public AttachmentModel(string imageUrl)
        {
            Fallback = "Image preview";
            ImageUrl = imageUrl;
        }

        public AttachmentModel(string fallback, string pretext)
        {
            Fallback = fallback;
            PreText = pretext;
        }
    }
}
