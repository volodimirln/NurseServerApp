using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
public class Messages : BackgroundService
{
    private static async Task GetMessage(Socket socket)
    {
        var response = new List<byte>();
        var bytesRead = new byte[1];
        
        while (true)
        {
            while (true)
            {
                var count = socket.Receive(bytesRead);
                if (count == 0 || bytesRead[0] == '\n') break;
                response.Add(bytesRead[0]);
            }

            var data = Encoding.UTF8.GetString(response.ToArray());
            if (!string.IsNullOrWhiteSpace(data))
            {
                Console.WriteLine($"Ответ: {data}");
                await StartServer.SendMessage(StartServer.socket);
            }
            response.Clear();
        }
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GetMessage(StartServer.socket);
            }
            catch (Exception ex)
            {

            }
            await Task.Delay(5000);
        }
    }
}