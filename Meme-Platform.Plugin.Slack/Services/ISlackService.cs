using Meme_Platform.Core.Models;

namespace Meme_Platform.Plugin.Slack.Services
{
    public interface ISlackService
    {
        void SendNotification(PostModel post);
    }
}
