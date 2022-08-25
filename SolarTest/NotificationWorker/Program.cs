using System;
using System.Linq;
using System.Threading;
using Common.Models;
using Common.Services;

namespace NotificationWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var birthdaySerice = new BirthdayService(new BirthdayRepository(Common.Configuration.ConnectionString));
            var timeoutInMs = 10000;
            Notify(birthdaySerice, timeoutInMs);
        }

        private static void Notify(IBirthdayService birthdayService, int timeoutInMs)
        {
            while (true)
            {
                var birthdays = birthdayService.GetBirthdays();
                var names = birthdays.Select(x => x.FullName);
                foreach (var name in names)
                {
                    Console.WriteLine(name);
                }
                
                Thread.Sleep(timeoutInMs);
            }
        }
    }
}