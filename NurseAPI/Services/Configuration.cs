using System.Text.Json;
public class Configuration : BackgroundService
{
    public static string address = "";
    public static int port = 0;
    public static int delayServer = 0;
    public static int delayMessage = 0;
    public static int numberMessages = 0;
    public static bool developStatus = false;
    public static int serverPort = 5000;
    public static string password = "";

    private readonly IWebHostEnvironment _webHostEnvironment;
    public Configuration(IWebHostEnvironment webHostEnvironment) : base()
    {
        _webHostEnvironment = webHostEnvironment;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Конфигурация сервера");
        var filePath = Path.Combine(_webHostEnvironment.ContentRootPath + "/Data/config.json");
            using (StreamReader reader = new StreamReader(filePath))
            {
                string text =  reader.ReadToEnd();
                Config? config = JsonSerializer.Deserialize<Config>(text);
                address = config.address;
                port = config.port;
                delayServer = config.delayServer;
                delayMessage = config.delayMessage;
                numberMessages = config.numberMessages;
                developStatus = config.developStatus;
                serverPort = config.serverPort;
                password = config.password;
            }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                //await SendMessage(tcpListener);
            }
            catch (Exception ex)
            {

            }
            await Task.Delay(5000);
        }

    }
}


