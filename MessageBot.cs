using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ValyutaCompressorBot;
using System.ComponentModel.Design;
using System.Buffers.Text;

namespace SearchFileBot
{
    public class MessageBot
    {
        static string? bir = null;
        static string? ikki = null;
        static int qiymat=int.MinValue;
       

        public static async Task MessageTextControll(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Message.Type switch
            {
                MessageType.Text =>MessageTextAll(botClient, update,cancellationToken),
                _ => MessageBot.MessageOtherControll(botClient, update, cancellationToken),
            };
        }
        static async Task MessageTextAll(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message=update.Message;
            if (message.Text == "/start" )
            {
                bir = null;
                ikki = null;
                qiymat = int.MinValue;
              
                await ButtonsControl.StartButton(botClient,update, cancellationToken);

            }
            else if(message.Text== "ayirboshlashni boshlash")
            {
                bir = null;
                ikki = null;
                qiymat = int.MinValue;
                await ButtonsControl.Buttons(botClient, update, cancellationToken, "Valyutani kodini kiriting ");
            }
            else
            {
               
                if (bir != null)
                {
                    if(Int32.TryParse(message.Text, out qiymat))
                    {
                        await ButtonsControl.Buttons(botClient, update, cancellationToken, "Ayirboshlash uchun Valyutani kodini kiriting ");
                    }
                    else if (ikki == null)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                           chatId: update.Message.Chat.Id,
                           text: "Qiymatni to'g'ri kiriting  ",
                           cancellationToken: cancellationToken);
                    }
                    else
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                           chatId: update.Message.Chat.Id,
                           text: "Ayirboshlashni boshlash uchun tugmani bosing",
                           cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    await ButtonsControl.Buttons(botClient, update, cancellationToken, "Valyutani kodini kiriting ");
                }


            }
        }
        public static async Task MessageOtherControll(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                           chatId: update.Message.Chat.Id,
                           text: "Amal qo'llab quvvatlanmadi\n"
                           + "Ayirboshlashni boshlash uchun tugmani bosing",
                           cancellationToken: cancellationToken);
            return;
        }


        public static async Task CallbackQueryToString(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.CallbackQuery.Data != null )
            {
                if (bir == null) 
                {
                    bir = update.CallbackQuery.Data.ToString();


                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.From.Id,
                    text: "Qiymatni kiriting ",
                    cancellationToken: cancellationToken);
                }
                else
                {
                    ikki= update.CallbackQuery.Data.ToString();
                    List<Valyuta> Course = await ButtonsControl.NbuList();
                    string s = $"{Course.Find(c => c.code == bir).code} dan {Course.Find(c => c.code == ikki).code} ga o'tkazildi qiymat = " +
                        $"{qiymat * double.Parse(Course.Find(c => c.code == bir).cb_price) / double.Parse(Course.Find(c => c.code == ikki).cb_price)}";
                    Message sentMessage = await botClient.SendTextMessageAsync(
                           chatId: update.CallbackQuery.From.Id,
                           text: s,
                           cancellationToken: cancellationToken);

                }

            }
            else
            {
                await ButtonsControl.Buttons(botClient, update, cancellationToken, "Valyutani kodini kiriting ");
            }
        }
    }
}
