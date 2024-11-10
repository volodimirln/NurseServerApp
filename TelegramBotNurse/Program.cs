using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Net.Sockets;

public class Program
{
    private static ITelegramBotClient _botClient;

    private static ReceiverOptions _receiverOptions;
    private static NetworkStream _socket;
    private static List<string> ct = new List<string>();

    private static string[] _newOrderStr = { "", "", "" };

    private static bool indicateName;
    private static bool indicateQuestion;
    private static bool indicatePhone;
    private static Configuration config;
    private static async Task SendMessages(NetworkStream stream, string text)
    {
        byte[] requestData = Encoding.UTF8.GetBytes(text + "\n");
        await stream.WriteAsync(requestData);
        Console.WriteLine("Сообщение отправлено");
    }

    public static async Task ClearCount()
    {

        while (true)
        {
            if (DateTime.Now.Subtract(new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 20, 0)).TotalSeconds > 0)
            {
                Console.WriteLine("Сброс ограничений");
            }
            await Task.Delay(1000);
        }
    }

    private static async Task ms(NetworkStream stream, byte[] bytesRead)
    {
        try
        {
            while (true)
            {
                int bytes = await stream.ReadAsync(bytesRead);
                if (bytes != 0)
                {
                    string answer = Encoding.UTF8.GetString(bytesRead, 0, bytes);
                    string time = DateTime.Now.ToString("HH:mm");
                    try
                    {
                        RequestMessage message1 = JsonConvert.DeserializeObject<RequestMessage>(answer);
                        Console.WriteLine($"Заявка с сайта от - {message1.name}");
                        long chatId = config.chatId;
                        if (message1.name == "test")
                        {
                            chatId = config.devChatId;
                        }
                        await _botClient.SendTextMessageAsync(
                                        chatId, $"Имя: {message1.name} \n" +
                                        $"Услуга: {message1.service} \n" +
                                        $"Телефон: {message1.phone} \n" +
                                        $"Время: {time}"
                                        );
                        answer = "";
                        bytes = 0;


                    }
                    catch
                    {
                        await _botClient.SendTextMessageAsync(
                                       config.devChatId, $"{answer}"
                                       );
                        Console.WriteLine($"Заявка с сайта от - {answer}");
                    }

                    bytes = 0;
                }
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    static async Task Main()
    {

        config = new Configuration();
        _botClient = new TelegramBotClient(config.telegramToken);
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] 
            {
                UpdateType.Message,
            },
            ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

        var me = await _botClient.GetMeAsync();
        Console.WriteLine($"{me.FirstName} запущен!");

        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(config.addressClient), config.portClient);
        TcpClient tcpClient = new TcpClient(ipPoint);
        await tcpClient.ConnectAsync(IPAddress.Parse(config.address), config.port);


        var stream = tcpClient.GetStream();
        _socket = stream;
        var bytesRead = new byte[512];

        await ms(stream, bytesRead);
        await Task.Delay(-1);
    }
    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message;
                        var user = message.From;

                        Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        var chat = message.Chat;

                        switch (message.Type)
                        {
                            case MessageType.Text:
                                {
                                    if (message.Text == "/start")
                                    {
                                        await SendMessages(_socket, "Новый пользователь");
                                        await botClient.SendTextMessageAsync(
                                            chat.Id, $"Здравствуйте, {user.FirstName}! Вас приветствует бот медсистры Елены Слесаревой" +
                                            $"\n" +
                                            $"\n✨ Вы можете ознакомиться с услугами (/service) и узнать подробнее (/about)" +
                                            $"\nВсе контакты есть в тедеграм боте, просто отправьте команду /contact или напишите Контакты" +
                                            $"\n" +
                                            $"\n⚡️Записаться можно также через бота, просто отправьте команду /neworder или напишиете Записаться и выберете услугу" +
                                            $"\nЕсли остались вопросы или есть обратная связь по боту, напишите @ElenaMed_krasnodar" +
                                            $"\nСписок команд /help"
                                            ); ;
                                        return;
                                    }
                                    if (message.Text == "/about")
                                    {
                                        await botClient.SendTextMessageAsync(
                                           chat.Id, $"О себе" +
                                           $"\n" +
                                           $"\n✨ Медсестра Елена Слесарева" +
                                           $"\n" +
                                           $"\n⚡️Мой трудовой опыт работы прошёл в детской реанимации новорождённых. С помощью большого опыта работы, без труда попадаю в сложные вены. Работа с детьми и взрослыми." +
                                           $"\n" +
                                           $"\nЕсли остались вопросы, напишите @ElenaMed_krasnodar"
                                           ); ;
                                        return;
                                    }
                                    if (message.Text == "/contact" || message.Text.ToLower() == "контакты")
                                    {
                                        await botClient.SendTextMessageAsync(
                                          chat.Id, $"Контакты" +
                                          $"\n" +
                                          $"\n🌟 Телефон: " +
                                          $"\n🌟 Почта: " +
                                          $"\n🌟 Telegram: @ElenaMed_krasnodar" +
                                          $"\n" +
                                          $"\nОтвечу с до в буднии дни"
                                          ); ;
                                        return;
                                    }
                                    if (message.Text == "/help" || message.Text.ToLower() == "команды")
                                    {
                                        await botClient.SendTextMessageAsync(
                                         chat.Id, $"Команды\n" +
                                         $"\nКак пользоваться ботом:" +
                                         $"\n" +
                                         $"\n💉 /service - услуги " +
                                         $"\n" +
                                         $"\nПодробно расписана информоция о доступных услугах, все четко, кратко и поделу" +
                                         $"\n" +
                                         $"\n👩 /about - о себе" +
                                         $"\n" +
                                         $"\nРассказываю все как есть, про опыт работы, направления и другое" +
                                         $"\n" +
                                         $"\n📡 /contact - контакты" +
                                         $"\n" +
                                         $"\nВся информация, чтобы легко и быстро связаться любым из удобных способов" +
                                         $"\n" +
                                         $"\n🧑‍⚕️ /neworder - заявка" +
                                         $"\n" +
                                         $"\nВы можете оставить заявку для связи на сайте или сразу же в телеграм боте" +
                                         $"\n" +
                                         $"\n🆘 /help - команды" +
                                         $"\n" +
                                         $"\nНаходитесь сейчас в этом месте"
                                         ); 
                                        return;
                                    }
                                    if (message.Text == config.token)
                                    {
                                        var filePath = Path.Combine("Data/config.json");

                                        Config configuration = new Config();
                                        using (StreamReader reader = new StreamReader(filePath))
                                        {
                                            string text = reader.ReadToEnd();
                                            configuration = System.Text.Json.JsonSerializer.Deserialize<Config>(text);
                                            reader.Close();
                                        }

                                        configuration.chatId = Convert.ToInt64(chat.Id);
                                        configuration.dataConfig = DateTime.Now.ToString("HH:mm dd:MM:yyyy");
                                        using (StreamWriter writer = new StreamWriter(filePath))
                                        {
                                            writer.Write(System.Text.Json.JsonSerializer.Serialize<Config>(configuration));
                                            writer.Close();
                                        }
                                        config = new Configuration();
                                        await botClient.SendTextMessageAsync(
                                            chat.Id, "Токен авторизован"
                                            );
                                        return;
                                    }
                                    if (message.Text == "/neworder" || message.Text == "Записаться")
                                    {
                                        if (ct.Where(p => p.Contains(chat.Id.ToString())).Count() < config.numberMessages)
                                        {
                                            ct.Add(chat.Id.ToString());
                                            indicateName = true;
                                            await botClient.SendTextMessageAsync(
                                                chat.Id, $"\n🧑‍💻 Запись" +
                                                $"\n" +
                                                $"\n☎️ С Вами свяжутся в течении для подтверждения и уточнения заявки" +
                                                $"\n" +
                                                $"\nДавайте начнем:" +
                                                $"\nУкажите имя"
                                                );
                                            return;
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(
                                            chat.Id, "Количество заявок привышено"
                                            );
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(message.Text)
                                    || message.Text != "/start" || message.Text != "Записаться")
                                    {
                                        if (indicateName)
                                        {
                                            _newOrderStr[0] = message.Text;

                                            await botClient.SendTextMessageAsync(
                                            chat.Id, "❓Укажите инересующий вопрос"
                                            );
                                            indicateName = false;
                                            indicateQuestion = true;
                                            return;
                                        }
                                        else if (indicateQuestion)
                                        {
                                            _newOrderStr[1] = message.Text;
                                            await botClient.SendTextMessageAsync(
                                            chat.Id, "📱Укажите номер телефона"
                                            );
                                            indicateQuestion = false;
                                            indicatePhone = true;
                                            return;
                                        }
                                        else if (indicatePhone)
                                        {
                                            _newOrderStr[2] = message.Text;
                                            string time = DateTime.Now.ToString("HH:mm");
                                            await botClient.SendTextMessageAsync(
                                            chat.Id, $"Ваша заявка успешно прияната в работу!" +
                                            $" \n" +
                                            $"\nВ ближайшее время с Вами свяжуться" +
                                            $"Ваша заявка: Имя - {_newOrderStr[0]}, Вопрос - {_newOrderStr[1]}, Телефон - {_newOrderStr[0]}"
                                            );
                                            string data = JsonConvert.SerializeObject(new { name = _newOrderStr[0], service = _newOrderStr[1], phone = _newOrderStr[2] });
                                            RequestMessage message1 = JsonConvert.DeserializeObject<RequestMessage>(data);
                                            Console.WriteLine($"Заявка с бота от - {message1.name}");

                                            long chatId = config.chatId;
                                            if (message1.name == "test")
                                            {
                                                chatId = config.devChatId;
                                            }
                                            await _botClient.SendTextMessageAsync(
                                                                chatId, $"Имя: {message1.name} \n" +
                                                                $"Услуга: {message1.service} \n" +
                                                                $"Телефон: {message1.phone} \n" +
                                                                $"Ник: @{user.Username} \n" +
                                                                $"Время: {time}"
                                                                );
                                            await SendMessages(_socket, data);
                                            indicatePhone = false;


                                            return;
                                        }
                                        else
                                        {

                                        }

                                    }
                                    if (message.Text == "/service" || message.Text.ToLower() == "услуги")
                                    {
                                        WebClient Client = new WebClient();
                                        List<Service> services = JsonConvert.DeserializeObject<List<Service>>(Client.DownloadString(config.domain + "/Date/GetServices"));
                                        string data = "\nУслуги" +
                                            "\n";
                                        foreach (Service service in services)
                                        {
                                            data += $"\n⚡️{service.name}" +
                                                $"\nЧто входит в услугу:" +
                                                $"\n✔️ {service.Advantage1}" +
                                                $"\n✔️ {service.Advantage2}" +
                                                $"\n✔️ {service.Advantage3}" +
                                                $"\nСтоимость: от  {service.price}{service.valute}" +
                                                $"\n" +
                                                $"\n";

                                        }
                                        await _botClient.SendTextMessageAsync(
                                                                chat.Id, data);
                                    }
                                    

                                    return;
                                }

                            default:
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        $"\n😔Извините, но я Вас не понимаю, пожалуйста попробуйте еще раз" +
                                        $"\n" +
                                        $"\n🤔Пока, что я не умею выполнять данные команды, но научусь"); ;
                                    return;
                                }
                        }

                        return;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
