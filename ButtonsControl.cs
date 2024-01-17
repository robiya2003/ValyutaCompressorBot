using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;

namespace ValyutaCompressorBot
{
    public class ButtonsControl
    {
        public static async Task Buttons(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken,string s)
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://nbu.uz/exchange-rates/json/");
            var response = await httpClient.SendAsync(request);

            var body = await response.Content.ReadAsStringAsync();
            List<Valyuta>? Course = JsonConvert.DeserializeObject<List<Valyuta>>(body);



            var buttons = new List<List<InlineKeyboardButton>>();
            var button = new List<InlineKeyboardButton>();


            foreach (Valyuta valyuta in Course)
            {
                if (button.Count < 6)
                {
                    button.Add(InlineKeyboardButton.WithCallbackData(text:valyuta.code,callbackData: valyuta.code));
                }
                else
                {
                    buttons.Add(button);
                    button = new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(text: valyuta.code, callbackData: valyuta.code) };
                }
            }
            if (button.Count > 0)
            {
                buttons.Add(button);
            }
            await botClient.SendTextMessageAsync(
                chatId:update.Message.Chat.Id,
                text: s,
                replyMarkup:new InlineKeyboardMarkup(buttons),
                cancellationToken:cancellationToken);
        }
        public static async Task<List<Valyuta>> NbuList()
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://nbu.uz/exchange-rates/json/");
            var response = await httpClient.SendAsync(request);

            var body = await response.Content.ReadAsStringAsync();
            List<Valyuta>? Course = JsonConvert.DeserializeObject<List<Valyuta>>(body);

            return Course;
        }
        public static async Task<bool> Cheacking(string c)
        {
            var lst=await NbuList();
            foreach (var item in lst)
            {
                if(item.code == c)
                {
                    return true;
                }
            }
            return false;
        }
        public static async Task StartButton(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                        {
                            new KeyboardButton[] { "ayirboshlashni boshlash" },
                        }
                    )

                {
                    ResizeKeyboard = true
                };


            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Assalomu Alekum Ayirboshlash uchun yordamchi bot" +
                "\nHar bir ayirboshlashdan oldin tugmani bosing",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
    }
}
