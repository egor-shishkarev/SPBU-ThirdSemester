using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleFTP;

public class Server
{
    private int port;

    private readonly TcpListener tcpListener;

    private TcpClient tcpClient;

    private StreamReader reader;

    private StreamWriter writer;
        
    public Server(int port)
    {
        this.port = port;

        tcpListener = new TcpListener(IPAddress.Any, this.port);

    }

    public async Task Start()
    {
        tcpListener.Start();

        tcpClient = tcpListener.AcceptTcpClient();

        Console.WriteLine($"Connection has been established with client {tcpClient.Client.LocalEndPoint}");

        reader = new StreamReader(tcpClient.GetStream());

        writer = new StreamWriter(tcpClient.GetStream());

        while (true)
        {
            string? data = null;

            while (string.IsNullOrEmpty(data))
            {
                if (tcpClient.Client.Connected)
                {
                    data = await reader.ReadLineAsync();
                }
            }

            Console.WriteLine($"Received data: {data}");
            await SendReponse(data.TrimEnd());
        }

    }

    private async Task SendReponse(string data)
    {
        var parsedData = data.Split();
        if (parsedData.Length != 2)
        {
            await writer.WriteLineAsync("Bad request!");
            await writer.FlushAsync();
        }
        else if (parsedData[0] != "1")
        {
            await writer.WriteLineAsync("Non available fuction");
            await writer.FlushAsync();
        }
        else
        {
            var path = parsedData[1];
            var response = new StringBuilder();

            Console.WriteLine("Ready to check directory");

            if (!Directory.Exists(path))
            {
                await writer.WriteAsync("-1 \n");
                await writer.FlushAsync();
            }
            else
            {
                var listOfFiles = Directory.GetFiles(path);

                var listOfDirectories = Directory.GetDirectories(path);

                await writer.WriteAsync($"{listOfFiles.Length + listOfDirectories.Length} ");

                foreach (var element in listOfFiles)
                {
                    await writer.WriteAsync(element.Replace("\\", "/") + " false ");
                }

                foreach (var element in listOfDirectories)
                {
                    await writer.WriteAsync(element.Replace("\\", "/") + " true ");
                }

                await writer.WriteAsync("\n");
                await writer.FlushAsync();
            }
        }
        
        Console.WriteLine("The message was sent");
    }
}
