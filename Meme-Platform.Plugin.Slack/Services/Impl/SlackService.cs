using Meme_Platform.Core;
using Meme_Platform.Core.Models;
using Meme_Platform.Plugin.Slack.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Meme-Platform.Plugin.Slack.Tests")]
namespace Meme_Platform.Plugin.Slack.Services.Impl
{
    internal class SlackService : ISlackService
    {
        private readonly IConfiguration configuration;
        private readonly ITemplateEngine templateEngine;

        public SlackService(IConfiguration configuration, ITemplateEngine templateEngine)
        {
            this.configuration = configuration;
            this.templateEngine = templateEngine;
        }

        public async Task SendPostMessage(PostModel model)
        {
            using (var client = SetupSlackClient())
            {
                var body = await templateEngine.RenderTemplate(
                    "new-post",
                    new KeyValuePair<string, string>("post-title", model.Title),
                    new KeyValuePair<string, string>("post-score", model.CalculateScore().ToString()));
                var content = new StringContent(body);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync("https://slack.com/api/chat.postMessage", content);
                var response = await result.Content.ReadAsJsonAsync<SlackResponseModel>();

                if (!response.Ok)
                {
                    throw new Exception($"Failed to notify SLACK for post {model.Id}\n" +
                        $"Slack server responed with HTTP {result.StatusCode} and ERR {response.Error}");
                }
            }
        }

        private HttpClient SetupSlackClient()
        {
            var handler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                ServerCertificateCustomValidationCallback = (m, cert, ch, err) => true
            };

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["Slack:OauthToken"]);

            return client;
        }
    }
}
