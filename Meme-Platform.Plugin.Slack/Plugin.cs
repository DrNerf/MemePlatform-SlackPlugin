using Meme_Platform.Core.Models;
using Meme_Platform.IL;
using Meme_Platform.IL.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meme_Platform.Plugin.Slack
{
    public class Plugin : IPlugin
    {
        public string GetName() => "Slack for Meme Platform";

        public string GetDescription() => "Provides integration for Slack. It will notify a slack channel when a new post is uploaded.";

        public string GetVersion() => "dev";

        public void ConfigureServices(IServiceCollection services)
        {

        }
    }

    public class PostCreatedEventHandler : IPostCreatedEventHandler
    { 

        public async void Execute(PostModel payload)
        {
            
        }
    }
}
