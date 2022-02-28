using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotLite;

namespace _1
{
    class Program
    {
        private static TelegramBotClient client;
        Vars vars = new Vars();
        static void Main(string[] args)
        {
            client = new TelegramBotClient(Config.Token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.WriteLine("[Log]: Bot started");
            Console.ReadLine();
            client.StopReceiving();
        }

        private static async void OnMessageHandler(object? sender, MessageEventArgs e)
        {
            var msg = e.Message;
            RandomVzor rvz = new RandomVzor();
            Pady pads = new Pady();
            Random rnd = new Random();
            bool isVzor = false;  int vzor;  int pad;  string answer;
            if (msg.Text != null)
            {
                Console.WriteLine($"[Log]: Пришло новое сообщение! От:{msg.From.FirstName} {msg.From.LastName} с текстом: {msg.Text}");
                switch (msg.Text)
                {
                    case "Vzor substantiv":
                        if (isVzor == false)
                        {
                            isVzor = true;
                            vzor = rnd.Next(0, 28); pad = rnd.Next(0, 7); answer = rvz.vzors[vzor,pad];
                            if (vzor >= 0 && vzor <= 13)
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, $"Склоните взор {rvz.vzors[vzor, 0]} в {pads.pady[pad]}, Singulár", replyToMessageId: msg.MessageId);
                            }
                            else if (vzor >= 14 && vzor <= 27)
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, $"Склоните взор {rvz.vzors[vzor - 14, 0]} в {pads.pady[pad]}, Plurál", replyToMessageId: msg.MessageId);
                            }
                        }
                        else
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, "Взор уже выдан, склоняй", replyMarkup: GetButtons());
                        }
                        //await client.SendTextMessageAsync(msg.Chat.Id, answer, replyToMessageId: msg.MessageId);
                        break;
                    case "/start":
                        await client.SendTextMessageAsync(msg.Chat.Id, "Кнопки обновлены", replyMarkup: GetButtons());
                        break;
                    case "Reload":
                        await client.SendTextMessageAsync(msg.Chat.Id, "Кнопки обновлены", replyMarkup: GetButtons());
                        break;
                    default:
                        if (isVzor == false)
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, "Неизвестная команда :(", replyToMessageId: msg.MessageId);
                        }
                        else if (isVzor == true)
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, "-", replyToMessageId: msg.MessageId);
                        }
                        break;
                }
            }
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = "Vzor substantiv"}, new KeyboardButton { Text = "Reload"} }
                }
            };
        }
    }
}