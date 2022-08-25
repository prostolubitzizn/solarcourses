using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;
using Microsoft.IdentityModel.Tokens;
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
        private static int timeOutInMs = 3000;
        private static string botToken = "5799312299:AAHsTeAwI7OS_8nNvv-osgstqQJjik7WPxA";
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
            
            _botClient = new TelegramBotClient(botToken);

            return _botClient;
        }

        private static async Task Bot()
        {
            var cts = new CancellationTokenSource();
            
            var botClient = BotInit(cts);
            
            var me = await botClient.GetMeAsync(cancellationToken: cts.Token);

            Console.WriteLine($"Start listening for @{me.Username}");
            
            await NotifySubscribedClientsInCycle(botClient, cts.Token);
           
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private static ITelegramBotClient BotInit(CancellationTokenSource cts)
        {
            var botClient = GetBotInstance();
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

            return botClient;
        }


        private static async Task NotifySubscribedClientsInCycle(ITelegramBotClient botClient, CancellationToken token)
        {
            while (true)
            {
                var birthdaySoonMessage = BirthdaySoonMessage();
                var todayBirthdayMessage = BirthdayTodayMessage();
            
                var chats = _chatService.GetAll();

                foreach (var chat in chats)
                {
                    await Notify(botClient, token, chat.ChatId, birthdaySoonMessage);
                    await Notify(botClient, token, chat.ChatId, todayBirthdayMessage);
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

            await Subscribe(botClient, cancellationToken, message);
        }

        private static async Task Subscribe(ITelegramBotClient botClient,CancellationToken cancellationToken, Message message)
        {
            if (message.Text != "/start")
            {
                return;
            }
            
            var chatId = message.Chat.Id;
            Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");
            SaveChatIfNotExist(chatId);
            await Notify(botClient, cancellationToken, chatId, "Subscription is successful. Wait for an updates");
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
            return Message("Birthday soon:", _birthdayService.GetSoonBirthdays());
        }

        private static string BirthdayTodayMessage()
        {
            return Message("Birthday today:", _birthdayService.GetTodaysBirthdays());
        }

        private static string Message(string template, IEnumerable<Birthday> birthdays)
        {
            var bdMessage = birthdays.Select(x => " | " + x.FullName + " | " + x.BirthDate.Date + " | ");
            
            
            var message = string.Join(System.Environment.NewLine, bdMessage);
            
            if (message.IsNullOrEmpty())
            {
                message = "Nobody";
            }
            
            return template + " " + message;
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