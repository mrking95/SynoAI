using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SynoAI.Models;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace SynoAI.Notifiers.Telegram
{
    public class Telegram : INotifier
    {
        public string ApiKey {get;set;}
        public string Destination {get;set;}

        public async Task Send(Camera camera, string filePath, IEnumerable<string> foundTypes, ILogger logger)
        {
            TelegramBotClient bot = new TelegramBotClient(ApiKey);
            
            Task sendMessage = bot.SendTextMessageAsync(Destination, "Test");
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                Task sendPhoto = bot.SendPhotoAsync(Destination, new InputOnlineFile(fileStream));
                await Task.WhenAll(sendMessage, sendPhoto);
            }
        }
    }
}