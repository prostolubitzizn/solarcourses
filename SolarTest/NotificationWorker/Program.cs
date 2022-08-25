using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationWorker
{
    class Program
    {

        private static TelegramBotClient _botClient;
        private static IChatService _chatService;
        private static IBirthdayService _birthdayService;
        private static int timeOutInMs = 10000;
        static async Task Main(string[] args)
        {
            _birthdayService = new BirthdayService(new BirthdayRepository(Common.Configuration.ConnectionString));
            _chatService = new ChatService(new ChatRepository(Common.Configuration.ConnectionString));

            await Bot();
            while (true)
            {
                
            }
        }

        private static TelegramBotClient GetBotInstance()
        {
            if (_botClient != null)
            {
                return _botClient;
            }
            
            string token = "5799312299:AAHsTeAwI7OS_8nNvv-osgstqQJjik7WPxA";
            _botClient = new TelegramBotClient(token);

            return _botClient;
        }

        private static async Task Bot()
        {
            var botClient = GetBotInstance();
            
            using var cts = new CancellationTokenSource();
            
            var receiverOptions = new ReceiverOptions
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
            
            await NotifySubscribedClientsWithSoonBirthdays(botClient, cts.Token);
           
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }


        private static async Task NotifySubscribedClientsWithSoonBirthdays(ITelegramBotClient botClient, CancellationToken token)
        {
            while (true)
            {
                var birthdaySoonMessage = BirthdaySoonMessage();
            
                var chats = _chatService.GetAll();

                foreach (var chat in chats)
                {
                    await Notify(botClient, token, chat.ChatId, birthdaySoonMessage);
                }

                Thread.Sleep(timeOutInMs);
            }
        }
        
        
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            if (messageText == "/start")
            {
                var chatId = message.Chat.Id;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            
                SaveChatIfNotExist(chatId);

                await Notify(botClient, cancellationToken, chatId, "Subscription is succesfull. Wait for an updates");
            }
        }
        
        private static async Task Notify(ITelegramBotClient botClient, CancellationToken cancellationToken, long chatId, string message)
        {
            // Echo received message text
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    cancellationToken: cancellationToken);
        }

        private static string BirthdaySoonMessage()
        {
            var birthdaysSoon = _birthdayService.GetSoonBirthdays();

            var bdMessage = birthdaysSoon.Select(x => " | " + x.FullName + " | " + x.BirthDate.Date + " | ");
            var message = string.Join(System.Environment.NewLine, bdMessage);
            return $"Birthday soon: {message}";
        }


        private static void SaveChatIfNotExist(long chatId)
        {
            if (ThereIsNoSuchChatId(chatId))
            {
                Console.WriteLine($"There is no chat Id with Id {chatId}. Saving chatId in db.");
                _chatService.Create(new Common.Models.Chat
                {
                    ChatId = (int)chatId
                });
            }
            else
            {
                Console.WriteLine("This chat is already exist. No-op.");
            }
        }

        private static bool ThereIsNoSuchChatId(long chatId)
        {
            var allChats = _chatService.GetAll();

            return allChats
                .Select(x => x.ChatId)
                .All(cId => cId != chatId);
        }
        
        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
    }
}