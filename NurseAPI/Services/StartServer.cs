using System.Net;
using System.Net.Sockets;
using System.Text;


public class StartServer : BackgroundService
{
    public static string textMessage = "";
    public static Socket socket;
 
    public async static Task SendMessage(Socket tcpClient)
    {
        byte[] data = Encoding.UTF8.GetBytes(textMessage);
        await tcpClient.SendAsync(data);
        Console.WriteLine($"Клиенту {tcpClient.RemoteEndPoint} отправлены данные");
        textMessage = "";
    }

    public static bool IsNewMessages = false;
    public static bool IsServerReady = false;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Configuration.address), Configuration.port);
        using Socket tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        tcpListener.Bind(ipPoint);
        tcpListener.Listen();
        Console.WriteLine("Сервер запущен. Ожидание подключений... ");
        var tcpClient = await tcpListener.AcceptAsync();
        socket = tcpClient;
        SendMessage(tcpClient);
        // await Messages.GetMessage(tcpListener);//!!!
          //  textMessage = "Сервер запyщен";
        //IsNewMessages = true;
        //SendMessage(tcpClient);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // textMessage = "Сервер запyщен";
                // IsNewMessages = true;
                 //await SendMessage(tcpClient, "Сервер запущен");
                
                // no get messages
            }
            catch (Exception ex)
            {

            }
            await Task.Delay(Configuration.delayServer);
        }

    }
}


