using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Task11.TBot
{
    public class UserControl
    {
        private TelegramBotClient Client { get; }
        public UserState State { get; }
        private long ChatID { get; }

        public UserControl(TelegramBotClient client, long chatID)
        {
            Client = client;
            ChatID = chatID;
            State = new UserState();
        }

        public void IncomingMessage(string text)
        {
            switch (State.CurrentStep)
            {
                case Step.FIRST_CURR: FirstChoice(text); break;
                case Step.SECOND_CURR: SecondChoise(text); break;
                case Step.DATE: DateChoise(text); break;
                case Step.AMOUNT: AmountChoise(text); break;
                default: State.CurrentStep = Step.FIRST_CURR; break;
            }
        }

        private async void FirstChoice(string text)
        {
            if (!BuilderMessageContext.CurrenciesDesc.ContainsKey(text))
            {
                SendMessagesOneSecond("");
                return;
            }

            State.FirstCurrency = text;
            State.CurrentStep = Step.SECOND_CURR;
            await Client.SendTextMessageAsync(
                    ChatID,
                    BuilderMessageContext.BuildSelectMessage(Constants.SECOND_SELECT_HEADER),
                    replyMarkup: BuilderMessageContext.BuildСurrencyKeyboard(State.FirstCurrency));
        }

        private async void SecondChoise(string text)
        {
            if (!BuilderMessageContext.CurrenciesDesc.ContainsKey(text) || State.FirstCurrency == text)
            {
                SendMessagesOneSecond(State.FirstCurrency);
                return;
            }

            State.SecondCurrency = text;
            State.CurrentStep = Step.DATE;
            await Client.SendTextMessageAsync(
                    ChatID,
                    Constants.DATE_SELECT_HEADER,
                    replyMarkup: new ReplyKeyboardMarkup(new KeyboardButton("NOW")));
        }

        private async void DateChoise(string text)
        {
            if ("now" == text.ToLower())
            {
                State.DateNow = true;
            }
            else
            {
                try
                {
                    DateTime dt = DateTime.Parse(text);
                    if (dt.Date.Year < 2000 || dt > DateTime.Now)
                        throw new ArgumentException();

                    State.RateDate = dt;
                }
                catch
                {
                    await Client.SendTextMessageAsync(
                    ChatID, Constants.ERROR_DATE_FORMAT);
                    return;
                }
            }

            State.CurrentStep = Step.AMOUNT;
            await Client.SendTextMessageAsync(
                    ChatID, Constants.AMOUNT_SELECT_HEADER);
        }

        private void AmountChoise(string text)
        {
            try
            {
                State.Amount = double.Parse(text.Replace('.', ','));
            }
            catch
            {
                State.Amount = 1.0;
            }

            State.CurrentStep = Step.FIRST_CURR;
            State.Success = true;
        }

        public async void SendMessagesOneSecond(string except)
        {
            await Client.SendTextMessageAsync(
                    ChatID,
                    BuilderMessageContext.BuildSelectMessage(except == string.Empty ?
                    Constants.FIRST_SELECT_HEADER :
                    Constants.SECOND_SELECT_HEADER),
                    replyMarkup: BuilderMessageContext.BuildСurrencyKeyboard(except));
        }

        public async void SendFinshMessage()
        {
            await Client.SendTextMessageAsync(
                    ChatID,
                    Constants.FINISH_MESS_HEAD + State.FirstCurrency + "/" + State.SecondCurrency + ": " + State.Rate + "\n" +
                    State.Amount + " " + State.FirstCurrency + " = " + (State.Rate * State.Amount) + " " + State.SecondCurrency);
        }

        public async void SendErrorMessage()
        {
            await Client.SendTextMessageAsync(
                    ChatID, Constants.ERROR_HEADER);
        }
    }
}
