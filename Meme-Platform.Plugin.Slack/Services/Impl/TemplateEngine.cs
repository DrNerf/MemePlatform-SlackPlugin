using Meme_Platform.Core;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Meme-Platform.Plugin.Slack.Tests")]
namespace Meme_Platform.Plugin.Slack.Services.Impl
{
    internal class TemplateEngine : ITemplateEngine
    {
        private const string ArgumentTemplate = "#{0}#";

        private readonly UIConfig config;
        private readonly IConfiguration configuration;

        public TemplateEngine(UIConfig config, IConfiguration configuration)
        {
            this.config = config;
            this.configuration = configuration;
        }

        public async Task<string> RenderTemplate(string templateName, params KeyValuePair<string, string>[] arguments)
        {
            var templateStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"Meme_Platform.Plugin.Slack.Templates.{templateName}.json");
            var reader = new StreamReader(templateStream);
            var template = await reader.ReadToEndAsync();
            template = ReplaceArguments(template, arguments);

            return template;
        }

        private string ReplaceArguments(string template, KeyValuePair<string, string>[] arguments)
        {
            string result = template.Clone().ToString();
            foreach (var arg in arguments)
            {
                result = result.Replace(string.Format(ArgumentTemplate, arg.Key), arg.Value);
            }

            result = result.Replace(string.Format(ArgumentTemplate, "icon_url"), config.SiteFavicon);
            result = result.Replace(string.Format(ArgumentTemplate, "username"), config.SiteName);
            result = result.Replace(string.Format(ArgumentTemplate, "channel"), configuration["Slack:Channel"]);

            return result;
        }
    }
}
