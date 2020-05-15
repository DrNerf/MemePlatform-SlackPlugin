using Meme_Platform.Core.Models;
using Meme_Platform.Plugin.Slack.Services.Impl;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Meme_Platform.Plugin.Slack.Tests
{
    public class MainTests
    {
        private Mock<IConfiguration> configuration = new Mock<IConfiguration>();

        [SetUp]
        public void Setup()
        {
            configuration.SetupGet(c => c["Slack:OauthToken"]).Returns("xoxb-614896465106-1095311372384-S1qHB4YwBezFu299eNeRHx2x");
            configuration.SetupGet(c => c["Slack:Channel"]).Returns("C011NME30BZ");
            configuration.SetupGet(c => c["UI:Name"]).Returns("Meme Platform");
            configuration.SetupGet(c => c["UI:Favicon"]).Returns("https://s3.ap-south-1.amazonaws.com/isupportcause/uploads/overlay/isupportimg_1508500653815.png");
        }

        [Test]
        public void SendMessage()
        {
            IConfiguration configObject = configuration.Object;
            new SlackService(configObject, new TemplateEngine(new Core.UIConfig(configObject), configObject))
                .SendPostMessage(new PostModelMock()).Wait();
        }

        class PostModelMock : PostModel
        {
            public PostModelMock()
            {
                Title = "Post title";
            }

            public new int CalculateScore()
            {
                return 69;
            }
        }
    }
}