using Meme_Platform.Core.Models;
using Meme_Platform.IL;
using Meme_Platform.IL.Events;
using Meme_Platform.Plugin.Slack.Services;
using Meme_Platform.Plugin.Slack.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Meme_Platform.Plugin.Slack
{
    public class Plugin : IPlugin
    {
        public string GetName() => "Slack for Meme Platform";

        public string GetDescription() => "Provides integration for Slack. It will notify a slack channel via Incoming webhook when a new post is uploaded.";

        public string GetVersion() => "1.0.0";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISlackService, SlackService>();
        }
    }

    public class PostCreatedEventHandler : IPostCreatedEventHandler
    {
        private readonly ISlackService slackService;

        public PostCreatedEventHandler(ISlackService slackService)
        {
            this.slackService = slackService;
        }

        public void Execute(PostModel payload)
        {
            slackService.SendNotification(payload);
        }
    }
}
