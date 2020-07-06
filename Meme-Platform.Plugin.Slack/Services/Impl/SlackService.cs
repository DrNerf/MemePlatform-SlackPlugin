using Meme_Platform.Core.Models;
using Meme_Platform.Plugin.Slack.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Meme-Platform.Plugin.Slack.Tests")]
namespace Meme_Platform.Plugin.Slack.Services.Impl
{
    internal class SlackService : ISlackService
    {
        private const string TitleTemplate = "<{0}|{1}>";

        private readonly IConfiguration configuration;

        public SlackService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendNotification(PostModel post)
        {
            var hook = configuration["Slack:Hook"];
            if (!string.IsNullOrEmpty(hook))
            {
                string websiteUrl = configuration["UI:WebSiteHostUrl"];
                var payload = new SlackPayloadModel
                {
                    Text = string.Format(TitleTemplate,
                            $"{websiteUrl}/Posts/View/{post.Id}",
                            post.Title),
                    Username = configuration["UI:Name"],
                    Icon = configuration["UI:Favicon"]
                };

                AttachmentModel attachment = null;
                if (post.IsNSFW)
                {
                    payload.Text += "\nThe post is flagged as NSFW. There will not be any kind of preview in slack, please view the post by following the provided link.";
                }
                else if (post.Content.ContentType == ContentType.Image)
                {
                    // Yeah I know I'm disgusting :(
                    if (websiteUrl != "http://localhost")
                    {
                        attachment = new AttachmentModel(
                                        $"{websiteUrl}/image/{post.Content.Id}{post.Content.Extension}"); 
                    }
                    else
                    {
                        attachment = new AttachmentModel(
                                        $"https://steamuserimages-a.akamaihd.net/ugc/934963999954440659/F7EA1482B7967952540BB8F0CEE0822048BDE466/");
                    }
                }
                else
                {
                    var videoUrl = Encoding.UTF8.GetString(post.Content.Data);
                    payload.Text += $"\n{videoUrl}";
                }

                if (attachment != null)
                {
                    payload.Attachments.Add(attachment);
                }

                using (var client = new HttpClient())
                {
                    // We can sync the task right here. The Meme Platform will invoke this in a background thread anyway.
                    var result = client.PostAsJsonAsync(hook, payload).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return;
                    }

                    throw new Exception($"Slack responded with: {result.StatusCode} {result.ReasonPhrase}");
                }
            }

            throw new Exception("Cannot post on slack. No hook url configured!");
        }
    }
}
