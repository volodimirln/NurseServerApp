using Newtonsoft.Json;
using System.Net;

public class Configuration 
{
    public string address = "";
    public  int port = 0;
    public  string telegramToken = "";
    public  long chatId = 0;
    public  long devChatId = 0;
    public string domain = "";
    public int numberMessages = 0;
    public string token = "";
    public string dataConfig;
    public string password;
    public string addressClient;
    public int portClient;

    public Configuration(){

        Console.WriteLine("Конфигурация клиента");
        var filePath = Path.Combine("Data/config.json");



        var filePathToken = Path.Combine("Data/token.txt");



        var filePathTGToken = Path.Combine("Data/tgtoken.txt");
        using (StreamReader reader = new StreamReader(filePath))
        {
            string text =  reader.ReadToEnd();
            Config? config = System.Text.Json.JsonSerializer.Deserialize<Config>(text);
            address = config.address;
            port = config.port;
            chatId = config.chatId;
            devChatId = config.devChatId;
            domain = config.domain;
            numberMessages = config.numberMessages;
            password = config.password;
            addressClient = config.addressClient;
            portClient = config.portClient;
            string dt = JsonConvert.SerializeObject(new { password = config.password});
            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string tokenweb = cli.UploadString(domain + "/ServerCommunication/gentoken", dt);
            token = tokenweb;
            using (StreamWriter writeToken = new StreamWriter(filePathToken))
            {
                writeToken.Write(token);
                Console.WriteLine(token);
            }
            using (StreamReader readerToken = new StreamReader(filePathTGToken))
            {
                telegramToken = readerToken.ReadToEnd();
            }
            dataConfig = config.dataConfig;
        }
    }
}


    