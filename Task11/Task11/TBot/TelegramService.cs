using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Task11.Api;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Task11.TBot
{
    public class TelegramService : ITelegramService
    {
        private string Token { get; } = "1961626635:AAHoazzOmqi-WNn2Xm4hdChjpJ41_3oh9zQ";
        private Dictionary<long, UserControl> Chats { get; }
        private TelegramBotClient Client { get; set; }
        private IRequestService HttpService { get; }

        public TelegramService(IRequestService httpService)
        {
            Client = new TelegramBotClient(Token);
            Chats = new Dictionary<long, UserControl>();
            HttpService = httpService;
        }

        public void Start()
        {
            using CancellationTokenSource Cts = new CancellationTokenSource();

            Client.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                Cts.Token);
            Console.ReadLine();
            Cts.Cancel();
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);        // logger
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var chatID = update.Message.Chat.Id;
            if (Chats.ContainsKey(chatID))
            {
                UserControl control = Chats[chatID];
                control.IncomingMessage(update.Message.Text);

                if (control.State.Success)
                {
                    try
                    {
                        if (control.State.DateNow)
                        {
                            control.State.Rate = HttpService.GetRate(
                                control.State.FirstCurrency,
                                control.State.SecondCurrency)
                                .Result;
                        }
                        else
                        {
                            control.State.Rate = HttpService.GetRate(
                                control.State.FirstCurrency,
                                control.State.SecondCurrency,
                                control.State.RateDate)
                                .Result;
                        }
                        control.SendFinshMessage();
                    }
                    catch
                    {
                        control.SendErrorMessage();
                    }
                    control.State.Success = false;
                }
            }
            else
            {
                UserControl control = new UserControl(Client, chatID);
                Chats.Add(chatID, control);
                control.SendMessagesOneSecond("");
            }
        }
    }
}
