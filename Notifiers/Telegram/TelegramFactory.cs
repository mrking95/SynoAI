using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SynoAI.Notifiers.Telegram
{
    public class TelegramFactory: NotifierFactory
    {
        public override INotifier Create(ILogger logger, IConfigurationSection section)
        {
            string apiKey = section.GetValue<string>("ApiKey");
            string destination = section.GetValue<string>("Destination");
            logger.LogInformation("Processing Telegram Config", apiKey);

            return new Telegram()
            {
                ApiKey = apiKey,
                Destination = destination
            };
        }
    }
}