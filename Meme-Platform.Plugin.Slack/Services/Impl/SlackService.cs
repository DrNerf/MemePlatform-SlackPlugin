using Meme_Platform.Core;
using Meme_Platform.Core.Models;
using Meme_Platform.Plugin.Slack.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Meme-Platform.Plugin.Slack.Tests")]
namespace Meme_Platform.Plugin.Slack.Services.Impl
{
    internal class SlackService : ISlackService
    {
        private readonly IConfiguration configuration;
        private readonly UIConfig uiConfig;

        public SlackService(IConfiguration configuration, UIConfig uiConfig)
        {
            this.configuration = configuration;
            this.uiConfig = uiConfig;
        }

        public async Task SendPostMessage(PostModel model)
        {
            using (var client = SetupSlackClient())
            {
                SendMessageRequestModel request = new SendMessageRequestModel
                {
                    Text = model.Title,
                    IconUrl = uiConfig.SiteFavicon,
                    Username = uiConfig.SiteName,
                    Channel = configuration["Slack:Channel"]
                };
                var result = await client.PostAsJsonAsync("https://slack.com/api/chat.postMessage", request);
                var response = await result.Content.ReadAsJsonAsync<SlackResponseModel>();

                if (!response.Ok)
                {
                    throw new Exception($"Failed to notify SLACK channel {request.Channel} for post {model.Id}\n" +
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
