using Meme_Platform.Core.Models;
using Meme_Platform.Plugin.Slack.Services.Impl;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Text;

namespace Meme_Platform.Plugin.Slack.Tests
{
    public class MainTests
    {
        private Mock<IConfiguration> configuration = new Mock<IConfiguration>();

        [SetUp]
        public void Setup()
        {
            // Put a valid slack hook.
            configuration.SetupGet(c => c["Slack:Hook"]).Returns("");
            configuration.SetupGet(c => c["UI:Name"]).Returns("Meme Platform");
            configuration.SetupGet(c => c["UI:Favicon"]).Returns("https://s3.ap-south-1.amazonaws.com/isupportcause/uploads/overlay/isupportimg_1508500653815.png");
            configuration.SetupGet(c => c["UI:WebSiteHostUrl"]).Returns("http://localhost");
        }

        [Test]
        public void NotifyForImage()
        {
            IConfiguration configObject = configuration.Object;
            new SlackService(configObject).SendNotification(new PostModel 
            {
                Id = -1,
                Title = "Test post",
                Content = new ContentModel
                {
                    Id = -1,
                    Extension = ".gif",
                    ContentType = ContentType.Image
                }
            });
        }

        [Test]
        public void NotifyForYTVideo()
        {
            IConfiguration configObject = configuration.Object;
            new SlackService(configObject).SendNotification(new PostModel
            {
                Id = -1,
                Title = "Test post",
                Content = new ContentModel
                {
                    Id = -1,
                    Data = Encoding.UTF8.GetBytes("https://www.youtube.com/watch?v=iPrnduGtgmc"),
                    ContentType = ContentType.Youtube
                }
            });
        }

        [Test]
        public void NotifyForNSFW()
        {
            IConfiguration configObject = configuration.Object;
            new SlackService(configObject).SendNotification(new PostModel
            {
                Id = -1,
                Title = "Test post",
                IsNSFW = true,
                Content = new ContentModel
                {
                    Id = -1,
                    Extension = ".gif",
                    ContentType = ContentType.Image
                }
            });
        }
    }
}