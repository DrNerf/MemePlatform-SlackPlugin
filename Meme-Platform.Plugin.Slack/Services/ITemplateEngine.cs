using Meme_Platform.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meme_Platform.Plugin.Slack.Services
{
    public interface ITemplateEngine
    {
        Task<string> RenderTemplate(string templateName, params KeyValuePair<string, string>[] arguments);
    }
}
