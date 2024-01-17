using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using ValyutaCompressorBot;

namespace SearchFileBot
{
    public class ControlClass
    {
        #region
        public async Task EssentialFunction()
        {
            var botClient = new TelegramBotClient("6742576559:AAGQWLllv_O2K9APgG46WpG1K3Kd4Y-Tb58");
            using CancellationTokenSource cts = new();
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() 
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
        }
        #endregion
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message=>MessageBot.MessageTextControll(botClient, update,cancellationToken),
                UpdateType.CallbackQuery=>MessageBot.CallbackQueryToString(botClient,update,cancellationToken),
                _=>MessageBot.MessageOtherControll(botClient, update,cancellationToken),
            };
            try
            {
                await handler;
            }
            catch (Exception ex)
            {
            }
        }
        #region
        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        #endregion
    }
}
