using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static TelegramBotClient botClient;
    private static string[] retroAdvice = {
        "Эй, друг! Для яркого сайта используй <table> для верстки — сделай фон ярко-розовым с зелёными рамками!",
        "Добавь мерцающий GIF с танцующими котами: <img src='dance.gif' loop='infinite'> — это 90-е в действии!",
        "Не забудь <blink> тег для текста — пусть он мигает, как неоновые вывески!",
        "Верстай сайт на HTML 3.2: таблицы внутри таблиц — это круто для мобильности (ха-ха, мобильность в 90-х!)",
        "Для анимации используй <marquee> — текст будет двигаться, как в старых браузерах!"
    };

    static async Task Main()
    {
        botClient = new TelegramBotClient("8124603446:AAExJ9CuYAaXfglmtOHZFTMQmOLmXiFNUmU");

        var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cts.Token
        );

        Console.WriteLine("Бот запущен. Нажмите Ctrl+C для выхода.");
        Console.ReadLine();
        cts.Cancel();
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message || message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        switch (messageText.ToLower())
        {
            case "/start":
                await botClient.SendMessage(chatId, "Привет, веб-мастер 90-х! Я твой персональный программист. Пиши /advice для советов по GeoCities и GIF!");
                break;
            case "/advice":
                var randomAdvice = retroAdvice[new Random().Next(retroAdvice.Length)];
                await botClient.SendMessage(chatId, randomAdvice);
                break;
            default:
                await botClient.SendMessage(chatId, "Набери /advice, чтобы получить ретро-совет по сайтам и анимациям!");
                break;
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
        return Task.CompletedTask;
    }
}
